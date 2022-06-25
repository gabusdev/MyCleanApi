using Application.PermaNotifications.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Microsoft.AspNetCore.Authentication;

namespace WebApi.IntegrationTest
{
    public class NotificationControllerTest : BaseTest
    {
        public NotificationControllerTest(CustomWebApplicationFactory<Program> factory) : base(factory)
        {
        }
        [Fact]
        public async void BroadCastNotificationReturnsCorrectResponses()
        {
            var route = "/api/v2/notification";
            var client = _factory.CreateClient();
            var (berarToken, userId) = await TryLogin();
            var authHeader = $"Bearer {berarToken}";
            var formModel = new Dictionary<string, string>
            {
                { "message", "Test Notification"}
            };

            // Check That user is unauthorized
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + berarToken);
            var response = await client.PostAsJsonAsync($"{route}/broadcast", formModel);
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }
        
        [Fact]
        public async void GetNotificationsReturnsCorrectResponses()
        {
            var route = "/api/v2/notification";
            var client = _factory.CreateClient();
            var (berarToken, userId) = await TryLogin();
            var authHeader = $"Bearer {berarToken}";

            // Check That user is unauthorized
            var response = await client.GetAsync($"{route}/user/1234");
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);

            // Check That authorized User can access the endpoint but with empty response
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + berarToken);
            Assert.Equal(authHeader, client.DefaultRequestHeaders.Authorization!.ToString());
            
            response = await client.GetAsync($"{route}/user/1234");
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadAsAsync<List<NotificationDto>>();
            Assert.NotNull(result);
            Assert.Empty(result);

            // Check that response is not empty with existent User
            response = await client.GetAsync($"{route}/user/{userId}");
            response.EnsureSuccessStatusCode();

            result = await response.Content.ReadAsAsync<List<NotificationDto>>();
            Assert.NotEmpty(result);
        }
    }
}
