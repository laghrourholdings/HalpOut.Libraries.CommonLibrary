using CommonLibrary.Interfaces;

namespace CommonLibrary.Implementations;

public class ServiceBusMessage : IServiceBusMessage
{
    public string Descriptor { get; set; }
    public string Contract { get; set; }
    public string? Data { get; set; }
    public Guid? LogHandleId { get; set; }
}