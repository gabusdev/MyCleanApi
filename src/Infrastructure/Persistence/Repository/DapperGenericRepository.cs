using Application.Common.Pagination;
using Application.Common.Persistence;
using Dapper;
using Infrastructure.Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.Repository;

internal class DapperGenericRepository<T, TDto> : IGenericRepository<T, TDto> where T : class where TDto : class
{
    private readonly DapperContext _context;
    private readonly string _dbName = nameof(T);

    public DapperGenericRepository(DapperContext context)
    {
        _context = context;
    }
    public async Task<int> Count(Expression<Func<T, bool>> match)
    {
        int count = 0;
        
        var sql = $"select count(Id) from {_dbName}";

        using (var connection = _context.CreateConnection())
        {
            count = await connection.ExecuteAsync(sql);
        }
        
        return count;
    }

    public async Task Delete(object id)
    {
        var sql = $"delete from {_dbName} where Id = @id";

        using (var connection = _context.CreateConnection())
        {
            await connection.ExecuteAsync(sql, id);
        }
    }

    public void Delete(T t)
    {
        
    }

    public void DeleteRange(IEnumerable<T> entities)
    {
        throw new NotImplementedException();
    }

    public Task<bool> Exists(Expression<Func<T, bool>> match)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<TDto>> GetAsync(Expression<Func<T, bool>>? filter, Expression<Func<T, bool>>? orderBy, bool desc = false, string includeProperties = "")
    {
        throw new NotImplementedException();
    }

    public Task<TDto?> GetByIdAsync(object id, string includeProperties = "")
    {
        throw new NotImplementedException();
    }

    public Task<PagedList<TDto>> GetPagedAsync(PaginationParams pParams, Expression<Func<T, bool>>? filter, Expression<Func<T, bool>>? orderBy, bool desc = false, string includeProperties = "")
    {
        throw new NotImplementedException();
    }

    public Task<bool> InsertAsync(T t)
    {
        throw new NotImplementedException();
    }

    public void InsertRangeAsync(List<T> t)
    {
        throw new NotImplementedException();
    }

    public void Update(T t)
    {
        throw new NotImplementedException();
    }
}
