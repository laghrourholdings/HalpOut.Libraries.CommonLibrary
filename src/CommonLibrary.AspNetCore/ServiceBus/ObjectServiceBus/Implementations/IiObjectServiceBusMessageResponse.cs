using System.Net;
using CommonLibrary.AspNetCore.ServiceBus.Interfaces;
using CommonLibrary.Core;

namespace CommonLibrary.AspNetCore.ServiceBus.Implementations;

public class IiObjectServiceBusMessageResponse : IObjectServiceBusMessageResponse<IIObject>
{
    public string Descriptor { get; set; }
    public string Contract { get; set; }
    public string? Data { get; set; }
    public Guid? LogHandleId { get; set; }
    public IIObject? Subject { get; set; }
    public IEnumerable<IIObject>? Subjects { get; set; }
    public IServiceBusMessage InitialRequest { get; set; }
    public HttpStatusCode StatusCode { get; set; }
}