using System.Net;

namespace CommonLibrary.AspNetCore.ServiceBus;

public class ServiceBusRequestReponse<TOldSubject, TNewSubject>
    : IServiceBusRequestResponse<TOldSubject, TNewSubject>
{
    public string Descriptor { get; set; }
    public string Contract { get; set; }
    public Guid? LogHandleId { get; set; }
    public TNewSubject? Subject { get; set; }
    public IEnumerable<TNewSubject>? Subjects { get; set; }
    public IServiceBusRequest<TOldSubject>? InitialRequest { get; set; }
    public HttpStatusCode StatusCode { get; set; }
}