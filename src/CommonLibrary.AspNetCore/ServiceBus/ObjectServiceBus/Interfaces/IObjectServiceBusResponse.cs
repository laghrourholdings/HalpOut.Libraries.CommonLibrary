using CommonLibrary.Core;

namespace CommonLibrary.AspNetCore.ServiceBus.Interfaces;

public interface IObjectServiceBusResponse<TNewSubject> : 
    IServiceBusResponse<TNewSubject> where TNewSubject : IObject
{
    
}