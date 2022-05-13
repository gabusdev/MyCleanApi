using Application.Common.Interfaces;

namespace WebApi.Services
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

            return request.Path.Value?? "/";
        }
    }
}
