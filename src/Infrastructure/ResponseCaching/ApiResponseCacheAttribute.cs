using Application.Common.Caching;
using Application.Common.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.ResponseCaching
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class ApiResponseCacheAttribute: Attribute, IAsyncActionFilter
    {
        public int Duration { get; set; } = 60;
        public ResponseCacheLocation Location { get; set; } = ResponseCacheLocation.Any;

        public ApiResponseCacheAttribute() { }
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (context.HttpContext.Request.Method != HttpMethod.Get.Method)
            {
                await next();
                return;
            }
        
            #region Services
                var cacheService  = context.HttpContext.RequestServices.GetService<ICacheService>();
            
            if (cacheService == null)
                throw new InvalidOperationException("There is No Cache Service Available");

            var jobService = context.HttpContext.RequestServices.GetService<IJobService>();

            if (jobService == null)
                throw new InvalidOperationException("There is No Jobs Service Available");
            #endregion

            string cacheKey = GenerateCacheKeyFromRequest(context.HttpContext.Request);

            if (!context.HttpContext.Request.Headers.CacheControl.Contains("no-cache"))
            {
                var cacheResponse = await cacheService.GetAsync<string>(cacheKey);

                if (!string.IsNullOrEmpty(cacheResponse))
                {
                    var contentResult = new ContentResult
                    {
                        Content = cacheResponse,
                        StatusCode = 200
                    };

                    context.Result = contentResult;
                    GenerateCacheHeaders(context.HttpContext);
                    return;
                }
            }

            var result = await next();

            if (result.Result is ObjectResult objectResult && objectResult.StatusCode == 200)
            {
                await cacheService.SetAsync(cacheKey, objectResult.Value);
                
                jobService.Schedule(() => 
                    cacheService.RemoveAsync(cacheKey, new CancellationToken()),
                    TimeSpan.FromSeconds(Duration));
            }
            GenerateCacheHeaders(context.HttpContext);
        }

        private string GenerateCacheKeyFromRequest(HttpRequest request)
        {
            var keyBuilder = new StringBuilder();

            keyBuilder.Append(request.Path.ToString());

            foreach(var (query, value) in request.Query)
            {
                keyBuilder.Append($"|{query}-{value}");
            }

            return keyBuilder.ToString();
        }
        private void GenerateCacheHeaders(HttpContext httpContext)
        {
            var responseHeaders = httpContext.Response.Headers;

            var cacheControlBuilder = new StringBuilder();

            switch (Location)
            {
                case ResponseCacheLocation.Any:
                    cacheControlBuilder.Append("public,");
                    break;
                case ResponseCacheLocation.Client:
                    cacheControlBuilder.Append("private,");
                    break;
                case ResponseCacheLocation.None:
                    cacheControlBuilder.Append("no-cache,");
                    break;
            }

            cacheControlBuilder.Append($"max-age={Duration}");

            responseHeaders.Add("Cache-Control", cacheControlBuilder.ToString());
        }
        private void CreateResponse()
        {

        }
    }
}
