using Application.Common.Pagination;
using Application.Common.Persistence;
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
        public virtual Task<int> Count(Expression<Func<T, bool>> match)
        {
            throw new NotImplementedException();
        }
        public virtual Task<bool> Exists(Expression<Func<T, bool>> match)
        {
            throw new NotImplementedException();
        }

        public virtual Task<IEnumerable<T>> GetAsync(Expression<Func<T, bool>>? filter = null, Expression<Func<T, bool>>? orderBy = null, bool desc = false, string includeProperties = "")
        {
            throw new NotImplementedException();
        }

        public virtual Task<T?> GetByIdAsync(object id, string includeProperties = "")
        {
            throw new NotImplementedException();
        }

        public virtual Task<PagedList<T>> GetPagedAsync(PaginationParams pParams, Expression<Func<T, bool>>? filter = null, Expression<Func<T, bool>>? orderBy = null, bool desc = false, string includeProperties = "")
        {
            throw new NotImplementedException();
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
    }
}
