using System.Net;

namespace CommonLibrary.AspNetCore.ServiceBus;

public interface IServiceBusPayloadsResponse<TOldSubject, TSubject> : 
    IServiceBusPayloads<TSubject>
{
    public IServiceBusPayloads<TOldSubject> InitialPayload { get; set; }
    public HttpStatusCode StatusCode {get; set;}

}