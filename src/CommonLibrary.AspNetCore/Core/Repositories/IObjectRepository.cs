using CommonLibrary.Core;
using CommonLibrary.Logging;

namespace CommonLibrary.AspNetCore.Core;

public interface IObjectRepository<T> : IRepository<T> where T:IBusinessObject
{
}