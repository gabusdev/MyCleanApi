using Marvin.Cache.Headers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers.v2
{
    [ApiVersion("2")]
    public class TestController : VersionedApiController
    {
        [HttpGet("versions")]
        public string SayVersion()
        {
            return "Hello from Version 2";
        }
    }
}
