using Domain.Common;

namespace Domain.Entities
{
    public interface IApplicationUser: IHasDomainEvent
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
    }
}
