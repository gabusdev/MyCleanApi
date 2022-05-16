using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace WebApi.Controllers.Tests
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : BaseApiController
    {
        private readonly IStringLocalizer<TestController> _localizer;
        public TestController(IStringLocalizer<TestController> localizer)
        {
            _localizer = localizer;
        }
        
        [HttpGet]
        [SwaggerOperation("Localization Test","Se utiliza un Header para la localizacion")]
        public string Test1()
        {
            return _localizer["auth.faisled"].ToString();
        }
    }
}
