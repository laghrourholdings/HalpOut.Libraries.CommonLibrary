using Microsoft.Extensions.Configuration;
using CommonLibrary.AspNetCore.ServiceBus;
using CommonLibrary.AspNetCore.Settings;
using Microsoft.Extensions.Logging;

namespace CommonLibrary.AspNetCore.Logging;

public static class ServiceBusMessageExtentions
{
    public static ServiceBusLogContext<TServiceBusMessage> GetLogContext<TServiceBusMessage>
        (this TServiceBusMessage message,
        IConfiguration configuration, LogLevel severity) where TServiceBusMessage : IServiceBusMessage
    {
        ServiceSettings serviceSettings = configuration.GetSection(nameof(ServiceSettings)).Get<ServiceSettings>() ?? throw new InvalidOperationException("ServiceSettings is null");
        var serviceBusLogContext =  new ServiceBusLogContext<TServiceBusMessage>
        {
            Severity = severity,
            ServiceName = serviceSettings.ServiceName,
            Message =  message
        };
        return serviceBusLogContext;
    }
}