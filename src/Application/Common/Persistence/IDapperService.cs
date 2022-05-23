using Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Persistence
{
    public interface IDapperService
    {
        Task<IReadOnlyList<T>> QueryAsync<T>(string sql, object? @params = null)
            where T : class, IEntity;

        Task<T> QueryFirstOrDefaultAsync<T>(string sql, object? @params = null)
            where T : class, IEntity;

        Task<T> QuerySingleAsync<T>(string sql, object? @params = null)
            where T : class, IEntity;
    }
}
