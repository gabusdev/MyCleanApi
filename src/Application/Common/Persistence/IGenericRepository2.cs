using Application.Common.Pagination;
using Application.Common.Specification;
using Domain.Common.Contracts;

namespace Application.Common.Persistence;
public interface IGenericRepository2<T> where T : IEntity
{
    Task<IEnumerable<T>> GetAsync(IBaseSpecifications<T>? baseSpecifications = null);
    Task<T?> GetByIdAsync(object id);
    Task<int> Count(IBaseSpecifications<T>? baseSpecifications = null);

    Task<PagedList<T>> GetPagedAsync(PaginationParams pParams, IBaseSpecifications<T>? baseSpecifications = null);
    Task<PagedList<TDto>> GetPagedAsync<TDto>(PaginationParams pParams, IBaseSpecifications<T>? baseSpecifications = null) where TDto : IDto;

    IQueryable<T> GetQuery(IBaseSpecifications<T>? baseSpecifications = null);

    Task<bool> InsertAsync(T t);
    void InsertRangeAsync(IEnumerable<T> t);

    void Update(T t);

    Task Delete(object id);
    void Delete(T t);
    void DeleteRange(IEnumerable<T> entities);
}

