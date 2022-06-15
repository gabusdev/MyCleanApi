using Application.Common.Interfaces;
using Application.Common.Pagination;
using Application.Common.Persistence;
using Domain.Common.Contracts;
using Infrastructure.Common.Extensions;
using Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Infrastructure.Persistence.Repository
{
    internal class EFGenericRepository<T> : IGenericRepository<T> where T : class, IEntity
    {
        protected readonly ApplicationDbContext _context;
        protected readonly DbSet<T> _db;

        public EFGenericRepository(ApplicationDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _db = _context.Set<T>();
        }
        public virtual async Task<int> Count(Expression<Func<T, bool>>? match = null)
        {
            return match is null
                    ? await _db.CountAsync()
                    : await _db.CountAsync(match);
        }
        public virtual async Task<bool> Exists(Expression<Func<T, bool>> match)
        {
            return await _db.FirstOrDefaultAsync(match) is not null;

        }

        public virtual async Task<IEnumerable<T>> GetAsync(Expression<Func<T, bool>>? filter = null, string includeProperties = "")
        {
            IQueryable<T> query = _db;

            if (filter != null)
                query = query.Where(filter);

            query = AddIncludes(query, includeProperties);

            return await query.ToListAsync();
        }

        public virtual async Task<T?> GetByIdAsync(object id, string includeProperties = "")
        {
            IQueryable<T> query = _db;

            query = AddIncludes(query, includeProperties);

            return (await query.FirstOrDefaultAsync(i => i.Id == Convert.ToString(id))) ?? default;
        }

        public virtual async Task<PagedList<T>> GetPagedAsync
            (PaginationParams pParams, Expression<Func<T, bool>>? filter = null, string includeProperties = "")
        {
            IQueryable<T> query = _db;

            if (filter != null)
                query = query.Where(filter);

            query = AddIncludes(query, includeProperties);

            return await query.ToPagedListAsync(pParams);
        }
        public virtual async Task<PagedList<TDto>> GetPagedAsync<TDto>
            (PaginationParams pParams, Expression<Func<T, bool>>? filter = null, string includeProperties = "")
            where TDto : IDto
        {
            var source = await GetPagedAsync(pParams, filter, includeProperties);
            var result = source.AdaptPagedList<T, TDto>();
            return result;
        }

        public virtual async Task<bool> InsertAsync(T t)
        {
            var result = await _db.AddAsync(t);
            return result.State == EntityState.Added;
        }

        public virtual void InsertRangeAsync(List<T> t)
        {
            _db.AddRangeAsync(t);
        }

        public virtual void Update(T t)
        {
            _db.Attach(t);
            _context.Entry(t).State = EntityState.Modified;
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

        protected IQueryable<T> AddIncludes(IQueryable<T> query, string includeProperties)
        {
            foreach (var include in includeProperties.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(include).AsNoTracking();
            }
            return query;
        }

        public IQueryable<T> GetQuery()
        {
            return _db.AsQueryable();
        }

        public async Task<IEnumerable<T>> GetFromQueryAsync(IQueryable<T> query)
        {
            return await query.ToListAsync();
        }

        public async Task<IEnumerable<T>> GetOrderedByAsync<TKey>(Expression<Func<T, bool>>? filter = null, Expression<Func<T, TKey>>? orderBy = null, bool desc = false, string includeProperties = "")
        {
            IQueryable<T> query = _db;

            if (filter != null)
                query = query.Where(filter);

            query = AddIncludes(query, includeProperties);

            if (orderBy != null)
            {
                query = desc
                    ? query.OrderByDescending(orderBy)
                    : query.OrderBy(orderBy);
            }

            return await query.ToListAsync();
        }
    }
}
