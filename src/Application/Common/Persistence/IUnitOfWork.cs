using Application.Common.Exceptions.Exception_Tracking;
using Domain.Entities;
using Domain.Entities.JoinTables;

namespace Application.Common.Persistence;

public interface IUnitOfWork : IDisposable
{
    IGenericRepository<PermaNotification> Notifications { get; }
    IGenericRepository<UserNotification> UserNotifications { get; }
    IGenericRepository<ExceptionLog, Guid> ExceptionLogs { get; }

    Task<int> CommitAsync();
}

