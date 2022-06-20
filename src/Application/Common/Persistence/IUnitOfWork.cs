using Domain.Entities;
using Domain.Entities.JoinTables;

namespace Application.Common.Persistence;

public interface IUnitOfWork : IDisposable
{
    IGenericRepository<PermaNotification> Notifications { get; }
    IGenericRepository<UserNotification> UserNotifications { get; }

    Task<int> CommitAsync();
}

