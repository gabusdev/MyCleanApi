using Application.Common.Pagination;
using Domain.Common.Contracts;
using System.Linq.Expressions;

namespace Application.Common.Persistence;
/// <summary>
/// Represents the Generic Repository for a <see cref="IEntity"/> Type
/// with a <see cref="string"/> Primary Key Type
/// </summary>
/// <typeparam name="T"></typeparam>
public interface IGenericRepository<T> : IGenericRepository<T, string> where T : IEntity { }
/// <summary>
/// Represents the Generic Repository for a <see cref="IEntity"/> Type
/// with a <see cref="TE"/> Entity Primary Key Type
/// </summary>
/// <typeparam name="T">The Type of The IEntity</typeparam>
/// <typeparam name="ET">The Type of the IEntity Primary Key</typeparam>
public interface IGenericRepository<T, ET> where T : IEntity<ET>
{
    Task<IEnumerable<T>> GetAsync(
            Expression<Func<T, bool>>? filter = null, string includeProperties = "");
    Task<PagedList<T>> GetPagedAsync(
            PaginationParams pParams,
            Expression<Func<T, bool>>? filter = null, string includeProperties = "");
    Task<PagedList<TDto>> GetPagedAsync<TDto>(
            PaginationParams pParams,
            Expression<Func<T, bool>>? filter = null, string includeProperties = "") where TDto : IDto;
    Task<IEnumerable<T>> GetOrderedByAsync<TKey>(
        Expression<Func<T, bool>>? filter = null,
        Expression<Func<T, TKey>>? orderBy = null, bool desc = false,
        string includeProperties = "");

    Task<T?> GetByIdAsync(object id, string includeProperties = "");
    Task<bool> Exists(Expression<Func<T, bool>> match);
    Task<int> Count(Expression<Func<T, bool>> match);

    Task<bool> InsertAsync(T t);
    void InsertRangeAsync(List<T> t);

    void Update(T t);

    Task Delete(object id);
    void Delete(T t);
    void DeleteRange(IEnumerable<T> entities);

    IQueryable<T> GetQuery();
    Task<IEnumerable<T>> GetFromQueryAsync(IQueryable<T> query);
}

