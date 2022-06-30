using Application.Identity.Users.Commands.UpdateUser;
using Application.PermaNotifications.Commands.SendNotificationCommand;
using Infrastructure.Middlewares.ExceptionHandler;
using System.Net;

namespace WebApi.IntegrationTest.ControllerTests.ProfileControllerTests
{
    public class ValidationTests : BaseTest
    {
        private readonly string _bearer;
        private readonly string _userId;
        public ValidationTests(CustomWebApplicationFactory<Program> factory) : base(factory)
        {
            Route = "/api/v2/profile";
            (_bearer, _userId) = Task.Run(async () => await TryLogin()).Result;
        }

        [Fact]
        public async Task PutUpdateProfileValidatorWorksOk()
        {
            AuthorizeClient(_bearer);

            var userData = new UpdateUserCommand
            {
                CurrentUserId = _userId,
                Email = "",
                FirstName = "",
                Id = "",
                LastName = "",
                PhoneNumber = "",
                UserName = ""
            };

            var result = await _client.PutAsJsonAsync($"{Route}", userData);
            result.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            (await result.Content.ReadAsAsync<ErrorResult>()).Messages.Should().HaveCount(6);

        }
    }
}
