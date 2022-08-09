using System.Linq.Expressions;
using CommonLibrary.Entities.InternalService;

namespace CommonLibrary.Repository;

//TODO: Correctly implement this class.
public interface IObjectRepository<T> where T : IObject
{
    Task<IReadOnlyCollection<T>> GetAllAsync();
    Task<IReadOnlyCollection<T>> GetAllAsync(Expression<Func<T,bool>> filter);
    Task<T> GetAsync(Guid Id);
    Task<T> GetAsync(Expression<Func<T,bool>> filter);
    Task CreateAsync(T entity);
    Task UpdateAsync(T entity);
    Task DeleteAsync(T entity);
    Task SuspendAsync(T entity);
}