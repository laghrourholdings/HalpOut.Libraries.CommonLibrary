using System.Net;

namespace CommonLibrary.Interfaces;

public interface IServiceBusResponse<TOldSubject,TSubject> : 
    IServiceBusRequest<TSubject>
{
    public IServiceBusRequest<TOldSubject> InitialRequest { get; set; }
    public HttpStatusCode StatusCode {get; set;}

}