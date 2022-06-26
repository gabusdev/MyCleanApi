using Application.Identity.Tokens.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace WebApi.IntegrationTest.ControllerTests.TokenControllerTests
{
    public class TokenEnpointsTests : BaseTest
    {
        public TokenEnpointsTests(CustomWebApplicationFactory<Program> factory) : base(factory)
        {
            Route = "/api/v1/token";
        }
        [Fact]
        public async Task PostActionToGetTokenAndRefreshTokenWorking()
        {
            var response = await _client.PostAsJsonAsync(Route, new { email = "admin@mail.com", password = "admin" });
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var token = await response.Content.ReadAsAsync<TokenResponse>();
            token.Should().NotBeNull();

            response = await _client.PostAsJsonAsync($"{Route}/refresh-token",
                new { Token = token.Token, RefreshToken = token.RefreshToken });
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            (await response.Content.ReadAsAsync<TokenResponse>()).Should().NotBeNull();
        }
    }
}
