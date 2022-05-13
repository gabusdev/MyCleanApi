using Application.Common.Interfaces;
using Infrastructure.Auth.Permissions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Authorization;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly IHttpContextService _httpContextService;
        private readonly ICurrentUser _cu;
        private static readonly string[] Summaries = new[]
        {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger,
            IHttpContextService httpContextService,
            ICurrentUser cu)
        {
            _logger = logger;
            _httpContextService = httpContextService;
            _cu = cu;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get()
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }

        [MustHavePermission(ApiAction.View, ApiResource.Tests)]
        [HttpGet("testing")]
        public string Test()
        {
            return _httpContextService.GetPath();
        }

        [Authorize]
        [HttpGet("testing2")]
        public List<string> Test2()
        {
            var lista = new List<string>();
            lista.Add(_cu.GetUserId());
            lista.Add(_cu.GetUserEmail());
            lista.Add(_cu.Name);
            lista.Add(User.GetUserId());
            return lista;
        }
    }
}