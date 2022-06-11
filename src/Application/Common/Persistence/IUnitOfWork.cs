using Domain.Entities;
using Domain.Entities.JoinTables;

namespace Application.Common.Persistence;

public interface IUnitOfWork : IDisposable
{
    IGenericRepository2<Notification> Notifications { get; }
    IGenericRepository2<UserNotification> UserNotifications { get; }

    Task<int> CommitAsync();
}

