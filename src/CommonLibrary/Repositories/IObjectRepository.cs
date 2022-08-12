using CommonLibrary.Core;

namespace CommonLibrary.Repositories;

public interface IObjectRepository<T> : IRepository<T> where T:IObject
{
}