using System.Net;

namespace CommonLibrary.AspNetCore.Core.ServiceBus;

public interface IServiceBusMessageResponse<TSubject> : 
    IServiceBusPayload<TSubject>
{
    public IServiceBusMessage InitialRequest { get; set; }
    public HttpStatusCode StatusCode {get; set;}

}