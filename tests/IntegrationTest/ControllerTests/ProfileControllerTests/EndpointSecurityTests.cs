using System.Net;

namespace WebApi.IntegrationTest.ControllerTests.ProfileControllerTests
{
    public class EndpointSecurityTests : BaseTest
    {
        public EndpointSecurityTests(CustomWebApplicationFactory<Program> factory) : base(factory)
        {
            Route = "/api/v2/profile";
        }

        [Theory]
        [InlineData("")]
        [InlineData("/notifications")]
        [InlineData("/permissions")]
        public async void GetForUnauhtenticatedUserResponseMustBe401(string endpoint)
        {
            var response = await _client.GetAsync($"{Route}{endpoint}");
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }
        [Theory]
        [InlineData("/change-password")]
        [InlineData("")]
        public async void PutForUnauhtenticatedUserResponseMustBe401(string endpoint)
        {
            var response = await _client.PutAsync($"{Route}{endpoint}", null);
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }
    }
}
