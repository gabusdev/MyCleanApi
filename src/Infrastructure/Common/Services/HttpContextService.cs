using Application.Common.Interfaces;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace Infrastructure.Common.Services
{
    public class HttpContextService : IHttpContextService
    {
        private readonly IHttpContextAccessor _requestAccesor;

        public HttpContextService(IHttpContextAccessor requestAccesor)
        {
            _requestAccesor = requestAccesor;
        }
        public string GetOrigin()
        {
            var context = _requestAccesor.HttpContext;
            if (context == null)
            {
                return "/";
            }
            var request = context.Request;
            return $"{request.Scheme}://{request.Host.Value}{request.PathBase.Value}";
        }

        public string GetPath()
        {
            var context = _requestAccesor.HttpContext;
            if (context == null)
            {
                return "/";
            }
            var request = context.Request;
            return request.Path.Value ?? "/";
        }

        public string GetRequestIpAddress()
        {
            var context = _requestAccesor.HttpContext;

            return context!.Request.Headers.ContainsKey("X-Forwarded-For")
            ? context.Request.Headers["X-Forwarded-For"]
            : context.Connection.RemoteIpAddress?.MapToIPv4().ToString() ?? "N/A";
        }

        public void AddHeaderValue(string headerName, object value)
        {
            var context = _requestAccesor.HttpContext ?? throw new Exception("No Http Context");
            var valueString =
                value.GetType() == typeof(string)
                ? value.ToString()
                : JsonConvert.SerializeObject(value);

            context.Response.Headers.Add(headerName, valueString);
        }
    }
}
