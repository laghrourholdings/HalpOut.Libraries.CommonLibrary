using CommonLibrary.Entities.InternalService;
using CommonLibrary.Interfaces;

namespace CommonLibrary.Implementations;

public class ObjectServiceBusRequest<TSubject> : IObjectServiceBusRequest<TSubject> where TSubject:IObject
{
    public string Contract { get; set; }
    public TSubject Subject { get; set; }
    public string? Descriptor { get; set; }
    public string? Data { get; set; }
    public Guid? LogHandleId { get; set; }
}