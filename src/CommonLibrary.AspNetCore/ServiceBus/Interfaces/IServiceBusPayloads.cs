namespace CommonLibrary.AspNetCore.ServiceBus;

public interface IServiceBusPayloads<TSubject> : IServiceBusMessage
{
    public IEnumerable<TSubject>? Subjects { get; set; }
}