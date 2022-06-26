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
using AutoFixture;
using Application.PermaNotifications.Commands.SendNotificationCommand;

namespace WebApi.IntegrationTest.ControllerTests.NotificationControllerTests
{
    public class EndpointResponseTests : BaseTest
    {
        private string _bearer;
        private string _originUserId;
        public EndpointResponseTests(CustomWebApplicationFactory<Program> factory) : base(factory)
        {
            Route = "/api/v2/notification";
            (_bearer, _originUserId) = Task.Run(async () => await TryLogin()).Result;
        }

        [Fact]
        public async void NotificationsControllerEndpointsWorksCorrectlly()
        {
            AuthorizeClient(_client, _bearer);

            await CheckWithNoMessages();

            await CheckCreationOfNotification();

            await CheckCreationOfNotificationBroadCast();

            var notId = await CheckWithNewMessage();
            
            await CheckSettingNotificationAsReaded(notId);

            await CheckNowMessageNotShowing();
        }

        private async Task CheckCreationOfNotification()
        {
            var (_, destUserId) = await TryLogin("guest@mail.com", "guest");
            var fixture = new Fixture();
            var notification = fixture.Create<SendNotificationCommand>();
            notification.DestinationUserId = destUserId;

            var response = await _client.PostAsJsonAsync($"{Route}", notification);
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            response = await _client.GetAsync($"{Route}/user/{destUserId}");
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadAsAsync<List<NotificationDto>>();
            Assert.NotEmpty(result);
            Assert.Contains(notification.Message, result.Select(r => r.Message).ToList());
        }

        private async Task CheckNowMessageNotShowing()
        {
            var response = await _client.GetAsync($"{Route}/user/{_originUserId}");
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
        private async Task CheckWithNoMessages()
        {
            // Check That authorized User can access the endpoint but with empty response
            var response = await _client.GetAsync($"{Route}/user/{_originUserId}");
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadAsAsync<List<NotificationDto>>();
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        private async Task<string?> CheckWithNewMessage()
        {
            // Check that response is not empty with existent User
            var response = await _client.GetAsync($"{Route}/user/{_originUserId}");
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadAsAsync<List<NotificationDto>>();
            Assert.NotEmpty(result);
            Assert.Contains("Test Notification", result.Select(r => r.Message).ToList());
            var notId = result.Find(r => r.Message == "Test Notification")?.Id;
            Assert.NotNull(notId);
            return notId;
        }
    }
}
