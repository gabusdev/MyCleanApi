using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace WebApi.IntegrationTest.SignalRTests
{
    public class NotificationTests : BaseTest
    {
        private readonly string _bearer;
        private readonly string _originUserId;
        public NotificationTests(CustomWebApplicationFactory<Program> factory) : base(factory)
        {
            (_bearer, _originUserId) = Task.Run(async () => await TryLogin()).Result;
        }

        [Fact]
        public async void ConnectsToNotificationHubWorks()
        {
            HubConnection connection = CreateHubConnection("notifications");

            var success = true;
            var message = "Testing Message";
            var response = string.Empty;

            connection.On<string>("EchoTesting", msg => response = msg);

            try
            {
                await connection.StartAsync();
                await connection.InvokeAsync("TestingEchoFunction", message);
                await connection.StopAsync();
            }
            catch
            {
                success = false;
            }

            Assert.True(success);
            Assert.Equal(message, response);
        }

        private HubConnection CreateHubConnection(string route)
        {
            var connection = new HubConnectionBuilder()
            .WithUrl($"http://localhost:5030/{route}", opt =>
            {
                opt.Headers.Add("Authorization", new AuthenticationHeaderValue("Bearer", _bearer).ToString());
                opt.HttpMessageHandlerFactory = _ => _factory.Server.CreateHandler();
            })
            .Build();
            return connection;
        }
    }
}
