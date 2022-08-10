using CommonLibrary.Entities.InternalService;

namespace CommonLibrary.Interfaces;

public interface IObjectServiceBusResponse<TOldSubject,TSubject> : 
    IServiceBusResponse<TOldSubject,TSubject> where TSubject : IObject
{
    
}