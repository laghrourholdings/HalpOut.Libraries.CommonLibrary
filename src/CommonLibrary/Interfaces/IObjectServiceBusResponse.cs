using CommonLibrary.Entities.InternalService;

namespace CommonLibrary.Interfaces;

public interface IObjectServiceBusResponse<TNewSubject> : 
    IServiceBusResponse<TNewSubject> where TNewSubject : IObject
{
    
}