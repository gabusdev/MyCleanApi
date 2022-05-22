using Application.Common.Persistence;
using Infrastructure.Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.Repository;

internal class UnitOfWork : IUnitOfWork
{
    // public IGenericRepository<Enitie> Entities { get; set; }

    public UnitOfWork(ApplicationDbContext context)
    {
        _context = context;
        // Entities ??= new EFGenericRepository<QueryRecord>(_context);
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

