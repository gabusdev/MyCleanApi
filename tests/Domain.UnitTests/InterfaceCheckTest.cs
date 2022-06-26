using Domain.Common.Contracts;
using Domain.Entities;
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
    }
}