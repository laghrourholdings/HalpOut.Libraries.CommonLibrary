namespace CommonLibrary.AspNetCore.ServiceBus;

public interface IServiceBusMessage
{
    public string Descriptor { get; set; }
    public string Contract { get; set; }
    public string? Data { get; set; }
    public Guid? LogHandleId { get; set; }
}
