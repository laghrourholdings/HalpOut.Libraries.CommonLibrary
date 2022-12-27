namespace CommonLibrary.AspNetCore.ServiceBus;

public class ServiceBusMessageContext : IServiceBusMessageContext
{
    public string Descriptor { get; set; }
    public string Contract { get; set; }
}