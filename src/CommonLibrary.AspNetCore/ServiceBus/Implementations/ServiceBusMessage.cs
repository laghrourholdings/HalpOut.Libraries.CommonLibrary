namespace CommonLibrary.AspNetCore.ServiceBus;

public class ServiceBusMessage : IServiceBusMessage
{
    public string Descriptor { get; set; }
    public string Contract { get; set; }
}