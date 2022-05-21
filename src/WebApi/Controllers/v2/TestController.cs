using Application.Common.Caching;
using Marvin.Cache.Headers;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers.v2
{
    [ApiVersion("2")]
    public class TestController : VersionedApiController
    {
        private readonly ICacheService _cache;
        public TestController(ICacheService cache)
        {
            _cache = cache;
        }
        
        [HttpGet("versions")]
        public string SayVersion()
        {
            return "Hello from Version 2";
        }
        [HttpGet("cache")]
        [HttpCacheIgnore]
        public async Task<string> CaheTest()
        {
            return await _cache.GetOrSetAsync("testeo", async () => await Task.Delay(5000).ContinueWith((t) => "Hola"));
        }
    }
}
