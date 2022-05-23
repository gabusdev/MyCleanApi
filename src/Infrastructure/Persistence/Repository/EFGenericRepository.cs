using Application.Common.Pagination;
using Application.Common.Persistence;
using Infrastructure.Common.Extensions;
using Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Infrastructure.Persistence.Repository
{
    internal class EFGenericRepository<T> : EFGenericRepository<T, T> where T : class{
        public EFGenericRepository(ApplicationDbContext context) : base(context)
        {
        }
        public override async Task<PagedList<T>> GetPagedAsync(PaginationParams pParams, Expression<Func<T, bool>>? filter = null, Expression<Func<T, bool>>? orderBy = null, bool desc = false, string includeProperties = "")
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

            return await query.ToPagedListAsync(pParams);
        }
    }

    internal class EFGenericRepository<T, TDto> : IGenericRepository<T, TDto> where T : class where TDto : class
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

        public virtual async Task<IEnumerable<TDto>> GetAsync(Expression<Func<T, bool>>? filter = null, Expression<Func<T, bool>>? orderBy = null, bool desc = false, string includeProperties = "")
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

            return (await query.ToListAsync()).Adapt<List<TDto>>();
        }

        public virtual async Task<TDto?> GetByIdAsync(object id, string includeProperties = "")
        {
            return (await _db.FindAsync(id)) is T t ? t.Adapt<TDto>(): default;
        }

        public virtual async Task<PagedList<TDto>> GetPagedAsync(PaginationParams pParams, Expression<Func<T, bool>>? filter = null, Expression<Func<T, bool>>? orderBy = null, bool desc = false, string includeProperties = "")
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

            return await query.ToPagedListAsync<T, TDto>(pParams);
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

        
    }
}
