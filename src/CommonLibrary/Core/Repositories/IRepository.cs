using System.Linq.Expressions;
using CommonLibrary.Logging;

namespace CommonLibrary.Core;

public interface IRepository<T>
{
    public Task<IEnumerable<T>> GetAllAsync();
    public Task<IEnumerable<T>> GetAllAsync(Expression<Func<T,bool>> filter);
    public Task<T?> GetAsync(Guid Id);
    public Task<T?> GetAsync(Expression<Func<T,bool>> filter);
    public Task CreateAsync(T entity);
    public Task RangeAsync(IEnumerable<T> entity);
    public Task UpdateAsync(T entity);
    public Task UpdateOrCreateAsync(T entity);
    
}