using Application.Common.Pagination;
using Mapster;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Pagination;

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

