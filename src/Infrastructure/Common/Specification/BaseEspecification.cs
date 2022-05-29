using Application.Common.Specification;
using System.Linq.Expressions;

namespace Infrastructure.Common.Specification;

internal class BaseSpecifications<T> : IBaseSpecifications<T>
{
    private readonly List<Expression<Func<T, object>>> _includeCollection = new List<Expression<Func<T, object>>>();

    public BaseSpecifications() { }

    public BaseSpecifications(Expression<Func<T, bool>> filterCondition)
    {
        FilterCondition = filterCondition;
    }

    public Expression<Func<T, bool>>? FilterCondition { get; private set; }
    public Expression<Func<T, object>>? OrderBy { get; private set; }
    public Expression<Func<T, object>>? OrderByDescending { get; private set; }
    public List<Expression<Func<T, object>>> Includes
    {
        get
        {
            return _includeCollection;
        }
    }

    public Expression<Func<T, object>>? GroupBy { get; private set; }

    protected void AddInclude(Expression<Func<T, object>> includeExpression)
    {
        Includes.Add(includeExpression);
    }

    protected void ApplyOrderBy(Expression<Func<T, object>> orderByExpression)
    {
        OrderBy = orderByExpression;
    }

    protected void ApplyOrderByDescending(Expression<Func<T, object>> orderByDescendingExpression)
    {
        OrderByDescending = orderByDescendingExpression;
    }

    protected void SetFilterCondition(Expression<Func<T, bool>> filterExpression)
    {
        FilterCondition = filterExpression;
    }

    protected void ApplyGroupBy(Expression<Func<T, object>> groupByExpression)
    {
        GroupBy = groupByExpression;
    }
}

