using CommonLibrary.Core;

namespace CommonLibrary.AspNetCore.ServiceBus.Interfaces;

public interface IObjectServiceBusMessageResponse<TNewSubject> : 
    IServiceBusMessageResponse<TNewSubject> where TNewSubject : IObject
{
    
}