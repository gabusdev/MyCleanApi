using Domain.Common.Contracts;
using Domain.Entities;
using Shared.Notifications;
using Xunit;

namespace Domain.UnitTests
{
    public class InterfaceCheckTest
    {
        [Fact]
        public void PermaNotificationEntityImplementsIHasDomainEvent()
        {
            var notification = new PermaNotification();
            Assert.True(notification is IHasDomainEvent);
        }
        [Fact]
        public void ConstatntNotificationFromServerValueCheck()
        {
            Assert.Equal("NotificationFromServer", NotificationConstants.NotificationFromServer);
        }
    }
}