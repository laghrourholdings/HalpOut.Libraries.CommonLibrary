using CommonLibrary.Core;

namespace CommonLibrary.AspNetCore;

public interface IObjectRepository<T> : IRepository<T> where T:IObject
{
    public Task DeleteAsync(T entity);
    public Task SuspendAsync(T entity);
}