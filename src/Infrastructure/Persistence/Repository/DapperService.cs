using Application.Common.Persistence;
using Dapper;
using Infrastructure.Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.Repository;

internal class DapperService : IDapperService
{
    private DapperContext _context;
    public DapperService(DapperContext context)
    {
        _context = context;
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
