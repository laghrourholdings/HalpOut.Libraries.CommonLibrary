using System.Linq.Expressions;

namespace CommonLibrary.Repositories;

public interface IRepository<T>
{
    Task<IEnumerable<T>> GetAllAsync();
    Task<IEnumerable<T>> GetAllAsync(Expression<Func<T,bool>> filter);
    Task<T> GetAsync(Guid Id);
    Task<T> GetAsync(Expression<Func<T,bool>> filter);
    Task CreateAsync(T? entity);
    Task UpdateAsync(T entity);
    Task DeleteAsync(T entity);
    Task SuspendAsync(T entity);
}