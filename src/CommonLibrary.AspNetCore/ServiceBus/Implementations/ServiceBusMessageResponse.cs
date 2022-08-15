using System.Net;

namespace CommonLibrary.AspNetCore.ServiceBus;

public class ServiceBusMessageReponse<TNewSubject>
    : IServiceBusMessageResponse<TNewSubject>
{
    public string Descriptor { get; set; }
    public string Contract { get; set; }
    public Guid? LogHandleId { get; set; }
    public TNewSubject? Subject { get; set; }
    public IEnumerable<TNewSubject>? Subjects { get; set; }
    public IServiceBusMessage? InitialRequest { get; set; }
    public HttpStatusCode StatusCode { get; set; }
}