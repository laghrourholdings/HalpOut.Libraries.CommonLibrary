using System.Net;

namespace CommonLibrary.AspNetCore.ServiceBus;

public class ServiceBusReponse<TNewSubject>
    : IServiceBusResponse<TNewSubject>
{
    public string Descriptor { get; set; }
    public string Contract { get; set; }
    public string? Data { get; set; }
    public Guid? LogHandleId { get; set; }
    public TNewSubject? Subject { get; set; }
    public IEnumerable<TNewSubject>? Subjects { get; set; }
    public IServiceBusMessage InitialRequest { get; set; }
    public HttpStatusCode StatusCode { get; set; }
}