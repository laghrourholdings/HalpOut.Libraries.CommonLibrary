using CommonLibrary.Core;

namespace CommonLibrary.AspNetCore;

public interface IObjectRepository<T> : IRepository<T> where T:IObject
{
    public Task BindLogHandle(Guid objectId, Guid logHandle);
    Task DeleteAsync(T entity);
    Task SuspendAsync(T entity);
}