namespace CommonLibrary.AspNetCore.Core.ServiceBus;

public interface IServiceBusPayload<TSubject> : IServiceBusMessage
{
    public TSubject? Subject { get; set; }
}