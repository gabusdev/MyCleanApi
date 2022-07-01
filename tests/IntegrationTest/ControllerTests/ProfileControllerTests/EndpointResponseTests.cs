using Application.Identity.Users.Commands.UpdateUser;
using Application.Identity.Users.Password.Commands.ChangePassword;
using Application.Identity.Users.Queries;
using Application.PermaNotifications.Queries;
using AutoFixture;
using System.Collections.Generic;
using System.Net.Http.Headers;

namespace WebApi.IntegrationTest.ControllerTests.ProfileControllerTests
{
    public class EndpointResponseTests : BaseTest
    {
        private readonly string _bearer;
        private readonly string _originUserId;
        public EndpointResponseTests(CustomWebApplicationFactory<Program> factory) : base(factory)
        {
            Route = "api/v1/profile";
            (_bearer, _originUserId) = Task.Run(async () => await TryLogin()).Result;
            AuthorizeClient(_bearer);
        }

        [Fact]
        public async Task GetProfileReturnsCorrectData()
        {
            var response = await _client.GetAsync(Route);
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsAsync<UserDetailsDto>();
            result.Should().NotBeNull();
            result.Id.Should().Be(_originUserId);
        }
        [Fact]
        public async Task PutUpdateProfileReturnsCorrectData()
        {
            var fixture = new Fixture();
            var newData = fixture.Create<UpdateUserRequest>();
            newData.Id = _originUserId;
            newData.Email = "admin@mail.com";
            newData.Image = null;
            newData.UserName = "testName";

            var response = await _client.PutAsJsonAsync(Route, newData);
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.NoContent);

            _client.DefaultRequestHeaders.CacheControl = new CacheControlHeaderValue { NoCache = true };
            response = await _client.GetAsync(Route);
            var result = await response.Content.ReadAsAsync<UserDetailsDto>();
            result.Should().NotBeNull();

            result.Id.Should().Be(_originUserId);
            result.Email.Should().Be(newData.Email);
            result.FirstName.Should().Be(newData.FirstName);
            result.LastName.Should().Be(newData.LastName);
            result.UserName.Should().Be(newData.UserName);
            result.PhoneNumber.Should().Be(newData.PhoneNumber);
        }

        [Fact]
        public async Task GetPermissionsWorksOk()
        {
            var response = await _client.GetAsync($"{Route}/permissions");
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsAsync<string[]>();
            result.Should().NotBeNull();
        }

        [Fact]
        public async Task PostChangePasswordWorksOk()
        {
            var testClient = _factory.CreateClient();
            var (testBearer, _) = await TryLogin("guest@mail.com", "guest");
            AuthorizeClient(testClient, testBearer);

            var newPass = "new123pass,";
            var newData = new ChangePasswordCommand
            {
                Password = "guest",
                NewPassword = newPass,
                ConfirmNewPassword = newPass
            };


            var response = await testClient.PutAsJsonAsync($"{Route}/change-password", newData);
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.NoContent);

            await TryLogin("guest@mail.com", newPass);
        }

        [Fact]
        public async Task GetNotificationsWorksOk()
        {
            var response = await _client.GetAsync($"{Route}/notifications");
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsAsync<List<NotificationDto>>();
            result.Should().NotBeNull();
        }
    }
}
