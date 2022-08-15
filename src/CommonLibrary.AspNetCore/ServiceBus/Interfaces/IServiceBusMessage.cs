namespace CommonLibrary.AspNetCore.ServiceBus;

public interface IServiceBusMessage
{
    public string Descriptor { get; set; }
    public string Contract { get; set; }
    public Guid? LogHandleId { get; set; }
}
