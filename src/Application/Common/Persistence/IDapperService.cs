using Domain.Common.Contracts;

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
