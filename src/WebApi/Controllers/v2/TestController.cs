using Application.Common.Caching;
using Application.Common.Persistence;
using Infrastructure.Identity.User;
using Marvin.Cache.Headers;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers.v2
{
    [ApiVersion("2")]
    public class TestController : VersionedApiController
    {
        private readonly ICacheService _cache;
        private readonly IDapperService _dapper;
        public TestController(ICacheService cache, IDapperService dapper)
        {
            _cache = cache;
            _dapper = dapper;
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
        [HttpGet("dapper")]
        public async Task<ActionResult> TestDapper()
        {
            var query = "select*from AspNetUsers u where u.Email = @mail";
            var param = new { mail = "admin@mail.com" };
            var result = await _dapper.QueryFirstOrDefaultAsync<ApplicationUser>(query, param);
            return Ok(result);
        }
    }
}
