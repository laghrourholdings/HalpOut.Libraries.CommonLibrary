﻿using System.Linq.Expressions;

namespace CommonLibrary.Core;

public interface IRepository<T>
{
    public Task<List<T>?> GetAllAsync();
    public Task<List<T>?> GetAllAsync(Expression<Func<T,bool>> filter);
    //public Task<T?> GetAsync(Key Id);
    public Task<T?> GetAsync(Expression<Func<T,bool>> filter);
    public Task CreateAsync(T entity);
    public Task RangeAsync(IEnumerable<T> entity);
    public Task UpdateAsync(T entity);
    public Task UpdateOrCreateAsync(T entity);
    
}