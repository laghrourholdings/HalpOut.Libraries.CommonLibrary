using System.Net;
using CommonLibrary.Interfaces;

namespace CommonLibrary.Implementations;

public class ServiceBusRequest<TSubject> : IServiceBusRequest<TSubject>
{
    public string Contract { get; set; }
    public string Descriptor { get; set; }
    public TSubject? Subject { get; set; }
    public IEnumerable<TSubject>? Subjects { get; set; }
    public string? Data { get; set; }
    public Guid? LogHandleId { get; set; }
}