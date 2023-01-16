namespace CommonLibrary.AspNetCore.Core.ServiceBus;

public class ServiceBusMessageContext : IServiceBusMessageContext
{
    public string Descriptor { get; set; }
    public string Contract { get; set; }
}