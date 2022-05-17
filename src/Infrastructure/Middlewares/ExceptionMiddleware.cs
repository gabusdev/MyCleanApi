using Application.Common.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Localization;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Serilog.Context;
using System.Net;

namespace Infrastructure.Middlewares;

internal class ExceptionMiddleware : IMiddleware
{
    private readonly ICurrentUserService _currentUser;
    private readonly IStringLocalizer<ExceptionMiddleware> _localizer;

    public ExceptionMiddleware(
        ICurrentUserService currentUser,
        IStringLocalizer<ExceptionMiddleware> localizer)
    {
        _currentUser = currentUser;
        _localizer = localizer;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (Exception exception)
        {
            string email = _currentUser.GetUserEmail() is string userEmail ? userEmail : "Anonymous";
            var userId = _currentUser.GetUserId();
            if (userId != string.Empty) LogContext.PushProperty("UserId", userId);
            LogContext.PushProperty("UserEmail", email);
            
            string errorId = Guid.NewGuid().ToString();
            LogContext.PushProperty("ErrorId", errorId);
            LogContext.PushProperty("StackTrace", exception.StackTrace);
            var errorResult = new ErrorResult
            {
                Source = exception.TargetSite?.DeclaringType?.FullName,
                Exception = exception.Message.Trim(),
                ErrorId = errorId,
                SupportMessage = _localizer["exceptionmiddleware.supportmessage"]
            };
            errorResult.Messages!.Add(exception.Message);
            var response = context.Response;
            response.ContentType = "application/json";
            if (exception is not CustomException && exception.InnerException != null)
            {
                while (exception.InnerException != null)
                {
                    exception = exception.InnerException;
                }
            }

            switch (exception)
            {
                case CustomException e:
                    response.StatusCode = errorResult.StatusCode = (int)e.StatusCode;
                    if (e.ErrorMessages is not null)
                    {
                        errorResult.Messages = e.ErrorMessages;
                    }

                    break;

                case KeyNotFoundException:
                    response.StatusCode = errorResult.StatusCode = (int)HttpStatusCode.NotFound;
                    break;

                default:
                    response.StatusCode = errorResult.StatusCode = (int)HttpStatusCode.InternalServerError;
                    break;
            }

            Log.Error($"{errorResult.Exception} Request failed with Status Code {context.Response.StatusCode} and Error Id {errorId}.");
            var json = JsonConvert.SerializeObject(errorResult, new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            });
            await response.WriteAsync(json);
        }
    }
}