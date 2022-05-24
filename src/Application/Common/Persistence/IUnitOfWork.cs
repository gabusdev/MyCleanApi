namespace Application.Common.Persistence;

public interface IUnitOfWork : IDisposable
{
    // IGenericRepository<Entitie> Entities { get; }

    Task<int> CommitAsync();
}

