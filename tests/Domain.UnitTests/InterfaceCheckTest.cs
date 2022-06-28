using Domain.Common.Contracts;
using Domain.Entities;
using Shared.Notifications;
using System.Linq;
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
        [Fact]
        public void Test()
        {
            string[] x = {"a", "b", "c"};
            System.Collections.Generic.List<string> y = x.ToList();
            int z = y.First() switch
            {
                "a" => 2,
                "b" => 3
            };
            Assert.Equal("2", System.Convert.ToString(z));
        }
    }
}