using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Caching.Memory;
using System.Text;

namespace Infrastructure.ResponseCaching
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class ApiResponseCacheAttribute : Attribute, IAsyncActionFilter
    {
        public int Duration { get; set; } = 60;
        public ResponseCacheLocation Location { get; set; } = ResponseCacheLocation.Any;

        public ApiResponseCacheAttribute() { }
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            // Exclude caching if is not Get Request
            if (context.HttpContext.Request.Method != HttpMethod.Get.Method)
            {
                await next();
                return;
            }

            // Get the caching service
            var cacheService = context.HttpContext.RequestServices.GetService<IMemoryCache>();

            if (cacheService == null)
            {
                throw new InvalidOperationException("There is No Memory Cache Service Available");
            }

            // Generate keys based on request endpoint
            string cacheKey = GenerateCacheKeyFromRequest(context.HttpContext.Request);
            string etagKey = GenerateETagKeyFromRequest(context.HttpContext.Request);
            string etagValue = context.HttpContext.Request.Headers.IfNoneMatch.ToString() ?? "";

            // Search for cahed key if no-cache header is present in request
            if (!context.HttpContext.Request.Headers.CacheControl.Contains("no-cache"))
            {
                // If there is an Etag for the current Request return 304
                if (ExistsEtagValueWithKey(etagKey, etagValue, cacheService))
                {
                    context.HttpContext.Response.StatusCode = 304;
                    return;
                }

                var cacheResponse = cacheService.Get<ApiResponseCacheObject>(cacheKey);

                // If there is Cache response for the Url Route,
                // if Authorization Headers Match, return cached response 
                // Ignore if Cache Type is public
                if (cacheResponse is not null)
                {
                    var requestAuth = context.HttpContext.Request.Headers.Authorization;
                    
                    if (Location == ResponseCacheLocation.Any || requestAuth == cacheResponse.Authorization)
                    {
                        context.Result = cacheResponse.Result;
                        RestoreResponseHeaders(context.HttpContext, cacheResponse.Headers);

                        return;
                    }
                }
            }

            // If not posible to retrieve cache continue and try to cache new response
            var result = await next();

            if (result.Result is ObjectResult objectResult && objectResult.StatusCode == 200)
            {
                GenerateCacheHeaders(context.HttpContext, out etagValue);

                var responseCahce = new ApiResponseCacheObject
                {
                    Result = objectResult,
                    Headers = context.HttpContext.Response.Headers.ToDictionary(
                        hd => hd.Key, hd => hd.Value.ToString()),
                    Authorization = Location == ResponseCacheLocation.Any 
                        ? string.Empty
                        : context.HttpContext.Request.Headers.Authorization
                };

                var cacheOptions = new MemoryCacheEntryOptions { AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(Duration) };
                cacheService.Set(cacheKey, responseCahce, cacheOptions);
                cacheService.Set(etagKey, etagValue, cacheOptions);
            }
        }

        private bool ExistsEtagValueWithKey(string etagKey, string etagValue, IMemoryCache cacheService)
        {
            if (string.IsNullOrEmpty(etagValue))
            {
                return false;
            }

            string cacheETagValue = cacheService.Get<string>(etagKey);
            if (string.IsNullOrEmpty(cacheETagValue) ||
                cacheETagValue != etagValue)
            {
                return false;
            }

            return true;
        }

        private void RestoreResponseHeaders(HttpContext httpContext, Dictionary<string, string> headers)
        {
            var responseHeaders = httpContext.Response.Headers;

            foreach (var header in headers)
            {
                responseHeaders.Add(header.Key, header.Value);
            }
        }

        private string GenerateCacheKeyFromRequest(HttpRequest request)
        {
            var keyBuilder = new StringBuilder();

            keyBuilder.Append(request.Path.ToString());

            foreach (var (query, value) in request.Query)
            {
                keyBuilder.Append($"|{query}-{value}");
            }

            return keyBuilder.ToString();
        }
        private string GenerateETagKeyFromRequest(HttpRequest request)
        {
            var keyBuilder = new StringBuilder("Etag-");

            keyBuilder.Append(request.Path.ToString());

            foreach (var (query, value) in request.Query)
            {
                keyBuilder.Append($"|{query}-{value}");
            }

            keyBuilder.Append($"~h:Auth:{(Location == ResponseCacheLocation.Any ? "" : request.Headers.Authorization)}");

            return keyBuilder.ToString();
        }
        private void GenerateCacheHeaders(HttpContext httpContext, out string eTag)
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
            eTag = GetEtagValue();
            responseHeaders.Add("Etag", eTag);
        }

        private string GetEtagValue()
        {
            var tag = Guid.NewGuid().ToString().Replace("-", "");
            return $"\"{tag.ToUpper()}\"";
        }
    }

}
