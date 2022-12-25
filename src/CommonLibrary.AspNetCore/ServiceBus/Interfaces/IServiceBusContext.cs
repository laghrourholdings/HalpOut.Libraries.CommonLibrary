namespace CommonLibrary.AspNetCore.ServiceBus;

public interface IServiceBusMessageContext
{
    public string Descriptor { get; set; }
    public string Contract { get; set; }
}
