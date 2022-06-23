using Application.Common.Persistence;
using Dapper;
using Infrastructure.Persistence.Context;

namespace Infrastructure.Persistence.Repository;

internal class DapperService : IDapperService
{
    private readonly DapperContext _context;
    public DapperService(DapperContext context)
    {
        _context = context;
    }

    public async Task<int> Execute(string sql, object? @params = null)
    {
        using (var connection = _context.CreateConnection())
        {
            var result = await connection.ExecuteAsync(sql, @params);
            return result;
        }
    }

    async Task<IReadOnlyList<T>> IDapperService.QueryAsync<T>(string sql, object? @params)
    {
        using (var connection = _context.CreateConnection())
        {
            var items = await connection.QueryAsync<T>(sql, @params);
            return items.ToList();
        }
    }

    async Task<T> IDapperService.QueryFirstOrDefaultAsync<T>(string sql, object? @params)
    {
        using (var connection = _context.CreateConnection())
        {
            var item = await connection.QueryFirstOrDefaultAsync<T>(sql, @params);
            return item;
        }
    }

    async Task<T> IDapperService.QuerySingleAsync<T>(string sql, object? @params)
    {
        using (var connection = _context.CreateConnection())
        {
            var item = await connection.QuerySingleAsync<T>(sql, @params);
            return item;
        }
    }
}
