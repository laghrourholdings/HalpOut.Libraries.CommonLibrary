namespace CommonLibrary.AspNetCore.Core.ServiceBus;

public class ServiceBusMessage : IServiceBusMessage
{
    public string Descriptor { get; set; }
    public string Contract { get; set; }
}