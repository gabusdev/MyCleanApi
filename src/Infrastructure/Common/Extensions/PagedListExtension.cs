using Application.Common.Pagination;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Common.Extensions;

public static class PagedListExtension
{
    public static async Task<PagedList<T>> ToPagedListAsync<T>(this IQueryable<T> source, PaginationParams pp)
    where T : class =>
        await source.ToPagedListAsync(pp.PageNumber, pp.PageSize);



    public static async Task<PagedList<T>> ToPagedListAsync<T>(this IQueryable<T> source, int pageNumber, int pageSize)
    where T : class
    {
        var count = source.Count();
        var items = await source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
        return new PagedList<T>(items, count, pageNumber, pageSize);
    }

    public static async Task<PagedList<TResponse>> ToPagedListAsync<T, TResponse>(this IQueryable<T> source, PaginationParams pp)
    where T : class
    where TResponse : class =>
        await source.ToPagedListAsync<T, TResponse>(pp.PageNumber, pp.PageSize);

    public static async Task<PagedList<TResponse>> ToPagedListAsync<T, TResponse>(this IQueryable<T> source, int pageNumber, int pageSize)
    where TResponse : class
    where T : class
    {
        var count = source.Count();
        var sourceItems = await source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
        var items = sourceItems.Adapt<List<TResponse>>();

        return new PagedList<TResponse>(items, count, pageNumber, pageSize);
    }

    public static PagedList<TResponse> AdaptPagedList<T, TResponse>(this PagedList<T> source)
        where TResponse : class
        where T : class
    {
        List<TResponse> items = new();
        foreach (var item in source)
        {
            items.Add(item.Adapt<TResponse>());
        }
        PagedList<TResponse> response = new(items, source.TotalCount, source.CurrentPage, source.PageSize);

        return response;
    }
}

