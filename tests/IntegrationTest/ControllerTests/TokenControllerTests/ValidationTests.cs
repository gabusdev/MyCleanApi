using Infrastructure.Middlewares.ExceptionHandler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace WebApi.IntegrationTest.ControllerTests.TokenControllerTests
{
    public class ValidationTests : BaseTest
    {
        public ValidationTests(CustomWebApplicationFactory<Program> factory) : base(factory)
        {
            Route = "/api/v1/token";
        }

        [Fact]
        public async Task PostActionToGetTokenValidatesCorrectlly()
        {
            var data = new { email = "", password = "" };
            var response = await _client.PostAsJsonAsync(Route, data);
            response.StatusCode.Should().Be(HttpStatusCode.UnprocessableEntity);
            (await response.Content.ReadAsAsync<ErrorResult>()).Messages.Should().HaveCount(2);

            data = new { email = "asd", password = "asd" };
            response = await _client.PostAsJsonAsync(Route, data);
            response.StatusCode.Should().Be(HttpStatusCode.UnprocessableEntity);
            (await response.Content.ReadAsAsync<ErrorResult>()).Messages.Should().HaveCount(1);

            data = new { email = "asd@ma.ca", password = "asd" };
            response = await _client.PostAsJsonAsync(Route, data);
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }
    }
}
