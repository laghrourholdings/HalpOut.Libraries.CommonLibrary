using CommonLibrary.AspNetCore.ServiceBus;
using Microsoft.Extensions.Logging;

namespace CommonLibrary.AspNetCore.Logging;

public interface IServiceBusLogContext<T> where T : IServiceBusMessage
{
    public LogLevel Severity { get; set; }
    public string ServiceName { get; set; }
    public T Message { get; set; }
}