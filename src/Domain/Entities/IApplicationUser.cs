namespace Domain.Entities
{
    public interface IApplicationUser : IAuditableEntity, IEntity
    {
        public string UserName { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
    }
}
