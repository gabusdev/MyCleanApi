﻿using Application.Common.Pagination;
using Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Persistence;
public interface IGenericRepository<T> where T : IEntity
{
    Task<IEnumerable<T>> GetAsync(
            Expression<Func<T, bool>>? filter,
            Expression<Func<T, bool>>? orderBy, bool desc = false,
            string includeProperties = "");
    Task<PagedList<T>> GetPagedAsync(
            PaginationParams pParams,
            Expression<Func<T, bool>>? filter,
            Expression<Func<T, bool>>? orderBy, bool desc = false,
            string includeProperties = "");
    Task<PagedList<TDto>> GetPagedAsync<TDto>(
            PaginationParams pParams,
            Expression<Func<T, bool>>? filter,
            Expression<Func<T, bool>>? orderBy, bool desc = false,
            string includeProperties = "")where TDto: IDto;
    Task<T?> GetByIdAsync(object id, string includeProperties = "");
    Task<bool> Exists(Expression<Func<T, bool>> match);
    Task<int> Count(Expression<Func<T, bool>> match);

    Task<bool> InsertAsync(T t);
    void InsertRangeAsync(List<T> t);

    void Update(T t);

    Task Delete(object id);
    void Delete(T t);
    void DeleteRange(IEnumerable<T> entities);

}

