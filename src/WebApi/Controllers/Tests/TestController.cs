using Application.Common.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace WebApi.Controllers.Tests
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : BaseApiController
    {
        private readonly IStringLocalizer<TestController> _localizer;
        private readonly IJobService _jobService;
        public TestController(IStringLocalizer<TestController> localizer, IJobService jobService)
        {
            _localizer = localizer;
            _jobService = jobService;
        }

        [HttpGet]
        [SwaggerOperation("Localization Test", "Se utiliza un Header para la localizacion")]
        public string Test1()
        {
            return _localizer["auth.faisled"].ToString();
        }

        [HttpGet("hangfiretest")]
        [SwaggerOperation("Localization Test", "Se utiliza un Header para la localizacion")]
        public string Test2()
        {
            
            return _jobService.Enqueue(() => SayHi());
        }
        [HttpGet("ignore")]
        public string SayHi() {
            var x = "Hola";
            Task.Delay(5000).ContinueWith((t) => {});
            return x;
        }

        [HttpGet("enviroment")]
        public string GetEnviroment()
        {
            var x = Environment.GetEnvironmentVariable("asd", EnvironmentVariableTarget.User);
            return x ?? "none";
        }
    }
}
