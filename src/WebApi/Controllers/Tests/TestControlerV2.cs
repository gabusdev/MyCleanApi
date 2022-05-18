using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers.Tests
{
    [Route("api/test")]
    [ApiController]
    [ApiVersion("2", Deprecated = false)]
    public class TestControlerV2 : ControllerBase
    {
        [HttpGet("versions")]
        public string SayVersion()
        {
            return "Hello from Version 2";
        }
    }
}
