using Application.Common.Exceptions.Exception_Tracking;
using Application.Common.Persistence;
using Domain.Entities;
using Domain.Entities.JoinTables;
using Infrastructure.Persistence.Context;

namespace Infrastructure.Persistence.Repository;

internal class UnitOfWork : IUnitOfWork
{
    public IGenericRepository<PermaNotification> Notifications { get; }
    public IGenericRepository<UserNotification> UserNotifications { get; }
    public IGenericRepository<ExceptionLog, string> ExceptionLogs { get; }

    public UnitOfWork(ApplicationDbContext context)
    {
        _context = context;
        Notifications ??= new EFGenericRepository<PermaNotification>(_context);
        UserNotifications ??= new EFGenericRepository<UserNotification>(_context);
        ExceptionLogs ??= new EFGenericRepository<ExceptionLog, string>(_context);
    }

    public async Task<int> CommitAsync()
    {
        return await _context.SaveChangesAsync();
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected void Dispose(bool disposing)
    {
        if (!this.disposed)
        {
            if (disposing)
            {
                _context.Dispose();
            }
            this.disposed = true;
        }
    }

    private readonly ApplicationDbContext _context;
    private bool disposed = false;
}

