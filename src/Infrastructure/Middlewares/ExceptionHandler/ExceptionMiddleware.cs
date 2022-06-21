using Application.Common.Exceptions.Exception_Tracking;
using Application.Common.Interfaces;
using Application.Common.Persistence;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Localization;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Serilog.Context;
using System.Net;

namespace Infrastructure.Middlewares.ExceptionHandler;

internal class ExceptionMiddleware : IMiddleware
{
    private readonly ICurrentUserService _currentUser;
    private readonly IStringLocalizer<ExceptionMiddleware> _localizer;
    private readonly IUnitOfWork _uow;

    public ExceptionMiddleware(
        ICurrentUserService currentUser,
        IStringLocalizer<ExceptionMiddleware> localizer,
        IUnitOfWork uow)
    {
        _currentUser = currentUser;
        _localizer = localizer;
        _uow = uow;
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
            if (userId != string.Empty)
            {
                LogContext.PushProperty("UserId", userId);
            }

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

            if (exception is not ValidationException)
            {
                Log.Error($"{errorResult.Exception} Request failed with Status Code {context.Response.StatusCode} and Error Id {errorId}.");
            }

            // Store ExceptionLog in Database
            await _uow.ExceptionLogs.InsertAsync(new ExceptionLog
                {
                    Id = errorResult.ErrorId,
                    Data = JsonConvert.SerializeObject(exception.Data, new JsonSerializerSettings
                    {
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                    }),
                    DataTime = DateTime.Now,
                    ErrorDescription = errorResult.Exception,
                    IPInfo = context.Connection.RemoteIpAddress?.MapToIPv4().ToString(),
                    Reference = context.Request.Path,
                    StackTrace = exception.StackTrace,
                    UserId = userId,
                    Source = errorResult.Source,
                    Messages = errorResult.Messages.Aggregate((a, b) => $"{a}\n{b}")
                });
            await _uow.CommitAsync();

            var json = JsonConvert.SerializeObject(errorResult, new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            });
            await response.WriteAsync(json);
        }
    }
}