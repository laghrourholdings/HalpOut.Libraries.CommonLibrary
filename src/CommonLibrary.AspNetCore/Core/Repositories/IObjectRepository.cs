using CommonLibrary.Core;

namespace CommonLibrary.AspNetCore.Core;

public interface IObjectRepository<T> : IRepository<T> where T:IBusinessObject
{
}