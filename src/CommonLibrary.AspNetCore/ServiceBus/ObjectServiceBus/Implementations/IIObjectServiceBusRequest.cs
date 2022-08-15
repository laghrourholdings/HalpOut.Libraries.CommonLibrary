using CommonLibrary.AspNetCore.ServiceBus.Interfaces;
using CommonLibrary.Core;

namespace CommonLibrary.AspNetCore.ServiceBus.Implementations;

public abstract class IIObjectServiceBusRequest : IObjectServiceBusRequest<IObject> 
{
    public string Descriptor { get; set; }
    public string Contract { get; set; }
    public string? Data { get; set; }
    public Guid? LogHandleId { get; set; }
    public IObject? Subject { get; set; }
    public IEnumerable<IObject>? Subjects { get; set; }
}