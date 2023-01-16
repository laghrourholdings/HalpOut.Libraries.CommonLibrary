namespace CommonLibrary.AspNetCore.Core.ServiceBus;

public interface IServiceBusPayloads<TSubject> : IServiceBusMessage
{
    public IEnumerable<TSubject>? Subjects { get; set; }
}