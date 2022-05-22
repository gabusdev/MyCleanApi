using Application.Common.Pagination;
using Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Persistence;

public interface IGenericRepository<T> where T : class
{
    Task<IEnumerable<T>> GetAsync(
            Expression<Func<T, bool>>? filter = null,
            Expression<Func<T, bool>>? orderBy = null, bool desc = false,
            string includeProperties = "");
    Task<PagedList<T>> GetPagedAsync(
            PaginationParams pParams,
            Expression<Func<T, bool>>? filter = null,
            Expression<Func<T, bool>>? orderBy = null, bool desc = false,
            string includeProperties = "");
    Task<T?> GetByIdAsync(object id, string includeProperties = "");
    Task<bool> Exists(Expression<Func<T, bool>> match);
    Task<int> Count(Expression<Func<T, bool>> match);

    Task InsertAsync(T t);
    Task InsertRangeAsync(List<T> t);

    Task<bool> Update(T t);

    Task<bool> Delete(object id);
    Task<bool> DeleteRange(IEnumerable<T> entities);

}

