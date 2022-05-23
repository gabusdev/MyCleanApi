using Application.Common.Pagination;
using Application.Common.Persistence;
using Infrastructure.Common.Extensions;
using Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Infrastructure.Persistence.Repository
{
    internal class EFGenericRepository<T> : IGenericRepository<T> where T : class
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

        public virtual async Task<IEnumerable<T>> GetAsync(Expression<Func<T, bool>>? filter = null, Expression<Func<T, bool>>? orderBy = null, bool desc = false, string includeProperties = "")
        {
            IQueryable<T> query = _db;
            
            if (filter != null)
                query = query.Where(filter);

            query = AddIncludes(query, includeProperties);

            if (orderBy != null)
                if (desc)
                    query = query.OrderByDescending(orderBy);
                else
                    query = query.OrderBy(orderBy);

            return await query.ToListAsync();
        }

        public virtual async Task<T?> GetByIdAsync(object id, string includeProperties = "")
        {
            return await _db.FindAsync(id);
        }

        public virtual async Task<PagedList<T>> GetPagedAsync(PaginationParams pParams, Expression<Func<T, bool>>? filter = null, Expression<Func<T, bool>>? orderBy = null, bool desc = false, string includeProperties = "")
        {
            IQueryable<T> query = _db;

            if (filter != null)
                query = query.Where(filter);

            query = AddIncludes(query, includeProperties);

            if (orderBy != null)
                if (desc)
                    query = query.OrderByDescending(orderBy);
                else
                    query = query.OrderBy(orderBy);

            return await query.ToPagedListAsync<T, T>(pParams.PageNumber, pParams.PageSize);
        }

        public virtual Task InsertAsync(T t)
        {
            throw new NotImplementedException();
        }

        public virtual Task InsertRangeAsync(List<T> t)
        {
            throw new NotImplementedException();
        }

        public virtual Task<bool> Update(T t)
        {
            throw new NotImplementedException();
        }
        public virtual Task<bool> Delete(object id)
        {
            throw new NotImplementedException();
        }

        public virtual Task<bool> DeleteRange(IEnumerable<T> entities)
        {
            throw new NotImplementedException();
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
    }
}
