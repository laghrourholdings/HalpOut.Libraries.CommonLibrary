using CommonLibrary.Core;

namespace CommonLibrary.AspNetCore.ServiceBus.Interfaces;

public interface IObjectServiceBusRequest<TSubject> 
    : IServiceBusRequest<TSubject> where TSubject : IObject
{
}