using System.Net;

namespace CommonLibrary.Interfaces;

public interface IServiceBusRequest<TSubject>
{
    public string Contract { get; set; }
    public TSubject Subject { get; set; }
    public string Descriptor { get; set; }
    
    public string? Data { get; set; }
    public Guid? LogHandleId { get; set; }
}