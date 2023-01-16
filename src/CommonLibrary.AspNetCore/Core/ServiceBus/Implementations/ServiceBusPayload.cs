namespace CommonLibrary.AspNetCore.Core.ServiceBus;

public class ServiceBusPayload<TSubject> : IServiceBusPayload<TSubject>
{
    public string Contract { get; set; }
    public string Descriptor { get; set; }
    public TSubject? Subject { get; set; }
    public Guid? LogHandleId { get; set; }
}