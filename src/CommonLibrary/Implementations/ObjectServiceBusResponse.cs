using System.Net;
using CommonLibrary.Entities.InternalService;
using CommonLibrary.Interfaces;

namespace CommonLibrary.Implementations;

public class ObjectServiceBusResponse<TOldSubject,TNewSubject>
    : IObjectServiceBusResponse<TOldSubject, TNewSubject> where TNewSubject : IObject 
{
    public string Contract { get; set; }
    public TNewSubject Subject { get; set; }
    public string Descriptor { get; set; }
    public string? Data { get; set; }
    public Guid? LogHandleId { get; set; }
    public IServiceBusRequest<TOldSubject> InitialRequest { get; set; }
    public HttpStatusCode StatusCode { get; set; }
}