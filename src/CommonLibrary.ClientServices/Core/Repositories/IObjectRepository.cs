using CommonLibrary.Core;

namespace CommonLibrary.ClientServices.Core;

public interface IObjectRepository<T> : IRepository<T> where T:IBusinessObject
{
    public Task BindLogHandle(Guid objectId, Guid logHandle);
}