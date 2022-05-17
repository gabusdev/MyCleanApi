namespace Infrastructure.Persistence.Initialization
{
    internal interface IApplicationDbInitializer
    {
        Task InitializeDatabaseAsync(CancellationToken cancellationToken);
    }
}
