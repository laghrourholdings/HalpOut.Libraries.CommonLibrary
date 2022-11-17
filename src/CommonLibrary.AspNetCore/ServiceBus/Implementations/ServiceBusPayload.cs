namespace CommonLibrary.AspNetCore.ServiceBus;

public class ServiceBusPayload<TSubject> : IServiceBusPayload<TSubject>
{
    public string Contract { get; set; }
    public string Descriptor { get; set; }
    public TSubject? Subject { get; set; }
    public IEnumerable<TSubject>? Subjects { get; set; }
    public Guid? LogHandleId { get; set; }
}