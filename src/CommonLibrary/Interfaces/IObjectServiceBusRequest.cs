using CommonLibrary.Entities.InternalService;

namespace CommonLibrary.Interfaces;

public interface IObjectServiceBusRequest<TSubject> 
    : IServiceBusRequest<TSubject> where TSubject : IObject
{
}