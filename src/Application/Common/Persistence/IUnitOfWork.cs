using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Persistence;

public interface IUnitOfWork : IDisposable
{
    // IGenericRepository<Entitie> Entities { get; }

    Task<int> CommitAsync();
}

