using Application.PermaNotifications.Commands.SendNotificationCommand;
using Infrastructure.Middlewares.ExceptionHandler;
using System.Net;

namespace WebApi.IntegrationTest.ControllerTests.NotificationControllerTests
{
    public class ValidationTests : BaseTest
    {
        private readonly string _bearer;
        private readonly string _userId;
        public ValidationTests(CustomWebApplicationFactory<Program> factory) : base(factory)
        {
            Route = "/api/v2/notification";
            (_bearer, _userId) = Task.Run(async () => await TryLogin()).Result;
        }

        [Fact]
        public async Task PostSendNotificationCommandValidatorReturnsException()
        {
            AuthorizeClient(_bearer);

            var notification = new SendNotificationCommand
            {
                Message = new string('x', 101),
                DestinationUserId = _userId
            };

            var result = await _client.PostAsJsonAsync($"{Route}", notification);
            result.StatusCode.Should().Be(HttpStatusCode.UnprocessableEntity);
            (await result.Content.ReadAsAsync<ErrorResult>()).Messages.Should().HaveCount(1);

            (notification.Message, notification.DestinationUserId) = ("asd", "123");
            result = await _client.PostAsJsonAsync($"{Route}", notification);
            result.StatusCode.Should().Be(HttpStatusCode.UnprocessableEntity);
            (await result.Content.ReadAsAsync<ErrorResult>()).Messages.Should().HaveCount(1);

            (notification.Message, notification.DestinationUserId) = ("", "123");
            result = await _client.PostAsJsonAsync($"{Route}", notification);
            result.StatusCode.Should().Be(HttpStatusCode.UnprocessableEntity);
            (await result.Content.ReadAsAsync<ErrorResult>()).Messages.Should().HaveCount(3);

            (notification.Message, notification.DestinationUserId) = ("asd", _userId);
            result = await _client.PostAsJsonAsync($"{Route}", notification);
            result.StatusCode.Should().Be(HttpStatusCode.OK);
        }
    }
}
