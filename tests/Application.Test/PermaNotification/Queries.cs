using MediatR;
using Xunit;

namespace Application.Test.PermaNotification
{
    public class Queries
    {
        public ISender Mediator { get; set; }
        public Queries(ISender mediator)
        {
            Mediator = mediator;
        }
        [Fact]
        public void GetNotificationsReturnsNotificationDtoList()
        {
            //var request = new GetUnreadedNotificationsByUserIdQuery()
            //var result = Mediator.Send()
        }
    }
}