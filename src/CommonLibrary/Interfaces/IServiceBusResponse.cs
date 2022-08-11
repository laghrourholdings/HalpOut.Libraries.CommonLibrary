using System.Net;

namespace CommonLibrary.Interfaces;

public interface IServiceBusResponse<TSubject> : 
    IServiceBusRequest<TSubject>
{
    public IServiceBusMessage InitialRequest { get; set; }
    public HttpStatusCode StatusCode {get; set;}

}