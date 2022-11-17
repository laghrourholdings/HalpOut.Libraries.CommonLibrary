using CommonLibrary.Core;

namespace CommonLibrary.AspNetCore.ServiceBus.Interfaces;

public interface IObjectServiceBusPayload<TSubject> 
    : IServiceBusPayload<TSubject> where TSubject : IObject
{
}