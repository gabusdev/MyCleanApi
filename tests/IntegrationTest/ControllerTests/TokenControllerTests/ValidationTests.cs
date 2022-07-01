using Infrastructure.Middlewares.ExceptionHandler;
using System.Net;

namespace WebApi.IntegrationTest.ControllerTests.TokenControllerTests
{
    public class ValidationTests : BaseTest
    {
        public ValidationTests(CustomWebApplicationFactory<Program> factory) : base(factory)
        {
            Route = "/api/v1/token";
        }

        [Fact]
        public async Task PostActionToGetTokenValidatesOk()
        {
            var data = new { email = "", password = "" };
            var response = await _client.PostAsJsonAsync(Route, data);
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            (await response.Content.ReadAsAsync<ErrorResult>()).Messages.Should().HaveCount(2);

            data = new { email = "asd", password = "asd" };
            response = await _client.PostAsJsonAsync(Route, data);
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            (await response.Content.ReadAsAsync<ErrorResult>()).Messages.Should().HaveCount(1);

            data = new { email = "asd@ma.ca", password = "asd" };
            response = await _client.PostAsJsonAsync(Route, data);
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }
        [Fact]
        public async Task PostActionToGetTokenFromRefreshValidatesOk()
        {
            var data = new { token = "", refreshtoken = "" };
            var response = await _client.PostAsJsonAsync($"{Route}/refresh-token", data);
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            (await response.Content.ReadAsAsync<ErrorResult>()).Messages.Should().HaveCount(2);

            data = new { token = "asd", refreshtoken = "asd" };
            response = await _client.PostAsJsonAsync($"{Route}/refresh-token", data);
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }
    }
}
