using Application.Common.Interfaces;
using Application.Identity.Users;
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
        private readonly IUserService _us;
        private static readonly string[] Summaries = new[]
        {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger,
            IHttpContextService httpContextService,
            ICurrentUser cu,
            IUserService us)
        {
            _logger = logger;
            _httpContextService = httpContextService;
            _cu = cu;
            _us = us;
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
            var x = _cu.GetUserClaims().Select(c => $"{c.Type}: {c.Value}").ToList();
            lista.Add(""+x.Count());
            lista.Add(_cu.GetUserId());
            lista.Add(_cu.GetUserEmail());
            lista.Add(_cu.Name);
            lista.Add(User.GetUserId());
            return x;
        }
        [HttpGet("testing3")]
        public async Task<List<string>> Test3()
        {
            var u = _cu.GetUserId();
            var x = await _us.GetRolesAsync(u, new CancellationToken());
            var z = x.Select(dto => dto.RoleName).ToList();
            z.Add(_cu.IsInRole(ApiRoles.Admin) ? "yes" : "no");
            return z;
        }
    }
}