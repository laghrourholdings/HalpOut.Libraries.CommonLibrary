using CommonLibrary.Entities.InternalService;

namespace CommonLibrary.Repositories;

public interface IObjectRepository<T> : IRepository<T> where T:IObject
{
}