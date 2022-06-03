using Application.Common.Specification;
using Domain.Common.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Common.Specification;
public class SpecificationEvaluator<TEntity> where TEntity : class, IEntity
{
    public static IQueryable<TEntity> GetQuery(IQueryable<TEntity> query, IBaseSpecifications<TEntity>? specifications)
    {
        // Do not apply anything if specifications is null
        if (specifications == null)
        {
            return query;
        }

        // Modify the IQueryable
        // Apply filter conditions
        if (specifications.FilterCondition != null)
        {
            query = query.Where(specifications.FilterCondition);
        }

        // Includes
        query = specifications.Includes
                    .Aggregate(query, (current, include) => current.Include(include).AsSplitQuery());

        // Apply ordering
        if (specifications.OrderBy != null)
        {
            query = query.OrderBy(specifications.OrderBy);
        }
        else if (specifications.OrderByDescending != null)
        {
            query = query.OrderByDescending(specifications.OrderByDescending);
        }

        // Apply GroupBy
        if (specifications.GroupBy != null)
        {
            query = query.GroupBy(specifications.GroupBy).SelectMany(x => x);
        }

        return query;
    }
}
