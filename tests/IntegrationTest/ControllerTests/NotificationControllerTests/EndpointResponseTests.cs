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
        public async void BroadCastNotificationAndMarkAsReadedWorksOk()
        {
            AuthorizeClient(_client, _bearer);

            // Send BroadCast Notification
            var response = await _client.PostAsJsonAsync($"{Route}/broadcast", new { message = "Test Notification" });
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

            // Get The Notifications and Look for The Broadcasted Notification
            response = await _client.GetAsync($"{Route}/user/{_originUserId}");
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadAsAsync<List<NotificationDto>>();
            Assert.Contains("Test Notification", result.Select(r => r.Message).ToList());
            var notId = result.Find(r => r.Message == "Test Notification")?.Id;
            Assert.NotNull(notId);

            // Mark the new Notification as Readed
            response = await _client.PutAsync($"{Route}/{notId}/readed", null);
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

            // Check now Notification is not visible (readed)
            response = await _client.GetAsync($"{Route}/user/{_originUserId}");
            response.EnsureSuccessStatusCode();

            result = await response.Content.ReadAsAsync<List<NotificationDto>>();
            Assert.DoesNotContain("Test Notification", result.Select(r => r.Message).ToList());
        }

        [Fact]
        public async Task PostNotificationAndGetNotificationWorksOk()
        {
            AuthorizeClient(_client, _bearer);
            
            // Create New Notificatiation
            var (_, destUserId) = await TryLogin("guest@mail.com", "guest");
            var fixture = new Fixture();
            var notification = fixture.Create<SendNotificationCommand>();
            notification.DestinationUserId = destUserId;

            // Send Notification
            var response = await _client.PostAsJsonAsync($"{Route}", notification);
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            // Check Notification in Destination User
            response = await _client.GetAsync($"{Route}/user/{destUserId}");
            response.EnsureSuccessStatusCode();

            // Check there is the new Notification
            var result = await response.Content.ReadAsAsync<List<NotificationDto>>();
            Assert.NotEmpty(result);
            Assert.Contains(notification.Message, result.Select(r => r.Message).ToList());
        }
    }
}
