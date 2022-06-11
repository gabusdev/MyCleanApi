using Application.Common.Interfaces;
using Application.Common.Pagination;
using Application.Common.Persistence;
using Application.Common.Specification;
using Domain.Common.Contracts;
using Infrastructure.Common.Extensions;
using Infrastructure.Common.Specification;
using Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repository;
internal class GenericRepository<T> : IGenericRepository2<T> where T : class, IEntity
{
    protected readonly ApplicationDbContext _context;
    protected readonly DbSet<T> _db;

    public GenericRepository(ApplicationDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _db = _context.Set<T>();
    }

    public virtual async Task<int> Count(IBaseSpecifications<T>? baseSpecifications = null)
    {
        var count = await SpecificationEvaluator<T>.GetQuery(_db, baseSpecifications).CountAsync();
        return count;
    }

    public virtual async Task Delete(object id)
    {
        var entity = await _db.FindAsync(id);
        Delete(entity);
    }
    public virtual void Delete(T? t)
    {
        if (t != null)
            _db.Remove(t);
    }

    public virtual void DeleteRange(IEnumerable<T> entities)
    {
        _db.RemoveRange(entities);
    }

    public virtual async Task<IEnumerable<T>> GetAsync(IBaseSpecifications<T>? baseSpecifications = null)
    {
        return await SpecificationEvaluator<T>.GetQuery(_db.AsNoTracking(), baseSpecifications).ToListAsync();
    }

    public virtual async Task<T?> GetByIdAsync(object id)
    {
        return await _db.FindAsync(id);
    }

    public virtual async Task<PagedList<T>> GetPagedAsync(PaginationParams pParams, IBaseSpecifications<T>? baseSpecifications = null)
    {
        return await SpecificationEvaluator<T>.GetQuery(_db.AsNoTracking(), baseSpecifications).ToPagedListAsync(pParams);
    }

    public virtual async Task<PagedList<TDto>> GetPagedAsync<TDto>(PaginationParams pParams, IBaseSpecifications<T>? baseSpecifications = null) where TDto : IDto
    {
        var source = await GetPagedAsync(pParams, baseSpecifications);
        var result = source.AdaptPagedList<T, TDto>();
        return result;
    }

    public IQueryable<T> GetQuery(IBaseSpecifications<T>? baseSpecifications = null)
    {
        return SpecificationEvaluator<T>.GetQuery(_db, baseSpecifications);
    }

    public virtual async Task<bool> InsertAsync(T t)
    {
        var result = await _db.AddAsync(t);
        return result.State == EntityState.Added;
    }

    public virtual void InsertRangeAsync(IEnumerable<T> t)
    {
        _db.AddRangeAsync(t);
    }

    public virtual void Update(T t)
    {
        _db.Attach(t);
        _context.Entry(t).State = EntityState.Modified;
    }
}

