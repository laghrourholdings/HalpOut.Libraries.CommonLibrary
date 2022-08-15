using System.Net;

namespace CommonLibrary.AspNetCore.ServiceBus;

public interface IServiceBusRequestResponse<TOldSubject, TSubject> : 
    IServiceBusRequest<TSubject>
{
    public IServiceBusRequest<TOldSubject> InitialRequest { get; set; }
    public HttpStatusCode StatusCode {get; set;}

}