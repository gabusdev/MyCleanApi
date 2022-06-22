using Domain.Common.Contracts;
using Domain.Entities.JoinTables;

namespace Domain.Entities
{
    public interface IApplicationUser : IAuditableEntity, IEntity
    {
        public string UserName { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public ICollection<UserNotification> UserNotifications { get; set; }
    }
}
