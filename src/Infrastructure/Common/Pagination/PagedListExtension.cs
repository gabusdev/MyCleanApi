using Application.Common.Pagination;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Common.Pagination;

public static class PagedListExtension
{
    public static async Task<PagedList<TResponse>> ToPagedListAsync<T, TResponse>(this IQueryable<T> source, int pageNumber, int pageSize)
    where TResponse : class
    where T : class
    {
        var count = source.Count();
        var sourceItems = await source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
        var items = sourceItems.Adapt<List<TResponse>>();

        return new PagedList<TResponse>(items, count, pageNumber, pageSize);
    }
}

