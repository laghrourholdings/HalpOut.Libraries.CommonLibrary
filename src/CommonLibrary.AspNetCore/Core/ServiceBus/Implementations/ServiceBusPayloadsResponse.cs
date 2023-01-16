using System.Net;

namespace CommonLibrary.AspNetCore.Core.ServiceBus;

public class ServiceBusPayloadsReponse<TOldSubject, TNewSubject>
    : IServiceBusPayloadsResponse<TOldSubject, TNewSubject>
{
    public string Descriptor { get; set; }
    public string Contract { get; set; }
    public Guid? LogHandleId { get; set; }
    public IEnumerable<TNewSubject>? Subjects { get; set; }
    public IServiceBusPayloads<TOldSubject>? InitialPayload { get; set; }
    public HttpStatusCode StatusCode { get; set; }
}