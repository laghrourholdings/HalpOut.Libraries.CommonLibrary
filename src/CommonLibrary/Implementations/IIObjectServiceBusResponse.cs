using System.Net;
using CommonLibrary.Entities.InternalService;
using CommonLibrary.Interfaces;

namespace CommonLibrary.Implementations;

public class ObjectServiceBusResponse
    : IObjectServiceBusResponse<IObject>
{
    public string Descriptor { get; set; }
    public string Contract { get; set; }
    public string? Data { get; set; }
    public Guid? LogHandleId { get; set; }
    public IObject? Subject { get; set; }
    public IEnumerable<IObject>? Subjects { get; set; }
    public IServiceBusMessage InitialRequest { get; set; }
    public HttpStatusCode StatusCode { get; set; }
}