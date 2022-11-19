namespace CommonLibrary.AspNetCore.ServiceBus;

public class ServiceBusPayloads<TSubject> : IServiceBusPayloads<TSubject>
{
    public string Contract { get; set; }
    public string Descriptor { get; set; }
    public IEnumerable<TSubject>? Subjects { get; set; }
    public Guid? LogHandleId { get; set; }
}