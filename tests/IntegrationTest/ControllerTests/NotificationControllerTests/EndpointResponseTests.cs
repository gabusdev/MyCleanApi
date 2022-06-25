using Application.PermaNotifications.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Microsoft.AspNetCore.Authentication;
using FluentAssertions;

namespace WebApi.IntegrationTest.ControllerTests.NotificationControllerTests
{
    public class EndpointResponseTests : BaseTest
    {
        public EndpointResponseTests(CustomWebApplicationFactory<Program> factory) : base(factory)
        {
            Route = "/api/v2/notification";
        }

        [Fact]
        public async void NotificationsControllerEndpointsWorksCorrectlly()
        {
            var (berarToken, userId) = await TryLogin();
            AuthorizeClient(_client, berarToken);

            await CheckWithNoMessages(userId);

            await CheckCreationOfNotificationBroadCast();

            var notId = await CheckWithNewMessage(userId);
            Assert.NotNull(notId);

            await CheckSettingNotificationAsReaded(notId);

            await CheckNowMessageNotShowing(userId);
        }

        private async Task CheckNowMessageNotShowing(string userId)
        {
            var response = await _client.GetAsync($"{Route}/user/{userId}");
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadAsAsync<List<NotificationDto>>();
            Assert.DoesNotContain("Test Notification", result.Select(r => r.Message).ToList());
        }

        private async Task CheckSettingNotificationAsReaded(string? notId)
        {
            var response = await _client.PutAsync($"{Route}/{notId}/readed", null);
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }

        private async Task CheckCreationOfNotificationBroadCast()
        {
            var formModel = new Dictionary<string, string>
            {
                { "message", "Test Notification"}
            };
            var response = await _client.PostAsJsonAsync($"{Route}/broadcast", formModel);
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }
        private async Task CheckWithNoMessages(string userId)
        {
            // Check That authorized User can access the endpoint but with empty response
            var response = await _client.GetAsync($"{Route}/user/{userId}");
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadAsAsync<List<NotificationDto>>();
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        private async Task<string?> CheckWithNewMessage(string userId)
        {
            // Check that response is not empty with existent User
            var response = await _client.GetAsync($"{Route}/user/{userId}");
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadAsAsync<List<NotificationDto>>();
            Assert.NotEmpty(result);
            Assert.Contains("Test Notification", result.Select(r => r.Message).ToList());
            return result.Find(r => r.Message == "Test Notification")?.Id;
        }
    }
}
