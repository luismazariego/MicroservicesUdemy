﻿namespace Ordering.Core.Repositories.Base
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;

    using Entities.Base;

    public interface IRepository<T> where T : Entity
    {
        Task<IReadOnlyList<T>> GetAllAsync();

        Task<IReadOnlyCollection<T>> GetAsync(Expression<Func<T, bool>> predicate);

        Task<IReadOnlyCollection<T>> GetAsync
            (
                Expression<Func<T, bool>> predicate = null,
                Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
                string includeString = null,
                bool disableTracking = true
            );

        Task<IReadOnlyCollection<T>> GetAsync
            (
                Expression<Func<T, bool>> predicate = null,
                Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
                List<Expression<Func<T, object>>> includes = null,
                bool disableTracking = true
            );

        Task<T> GetByIdAsync(int id);

        Task<T> AddAsync(T entity);

        Task UpdateAsync(T entity);

        Task DeleteAsync(T entity);
    }
}
