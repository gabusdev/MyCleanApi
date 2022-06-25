
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace WebApi.IntegrationTest.ControllerTests.NotificationControllerTests
{
    public class EndpointSecurityTests : BaseTest
    {
        private string _bearer;
        public EndpointSecurityTests(CustomWebApplicationFactory<Program> factory) : base(factory)
        {
            Route = "/api/v2/notification";
            (_bearer, _) = Task.Run(async () => await TryLogin()).Result;
        }

        [Theory]
        [InlineData("/user/123")]
        public async void GetForUnauhtenticatedUserResponseMustBe401(string endpoint)
        {
            var response = await _client.GetAsync($"{Route}{endpoint}");
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }
        [Theory]
        [InlineData("/123/readed")]
        public async void PutForUnauhtenticatedUserResponseMustBe401(string endpoint)
        {
            var response = await _client.PutAsync($"{Route}{endpoint}", null);
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }
        [Theory]
        [InlineData("")]
        [InlineData("/broadcast")]
        public async void PostForUnauhtenticatedUserResponseMustBe401(string endpoint)
        {
            var response = await _client.PostAsync($"{Route}{endpoint}", null);
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }
    }
}
