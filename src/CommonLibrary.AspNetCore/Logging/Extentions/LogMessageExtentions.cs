using CommonLibrary.AspNetCore.Contracts.Logging;
using Microsoft.Extensions.Configuration;
using CommonLibrary.AspNetCore.Settings;
using CommonLibrary.Logging;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace CommonLibrary.AspNetCore.Logging;

public static class LogMessageExtentions
{


    /*public static ServiceBusPayload<TLogMessage> GetContract<TLogMessage>
        (this TLogMessage logMessage, IConfiguration configuration) where TLogMessage : ILogMessage, new()
    {
        ServiceSettings serviceSettings = configuration.GetSection(nameof(ServiceSettings)).Get<ServiceSettings>() ?? throw new InvalidOperationException("ServiceSettings is null");
        logMessage.Descriptor = logMessage.Descriptor?.Insert(0,$"{serviceSettings.ServiceName} | ");
        var serviceBusPayload = new ServiceBusPayload<TLogMessage>
        {
            Subject = logMessage,
            Contract = nameof(CreateLogMessage),
            Descriptor = $"Creation of LogMessage {logMessage.Id}"
        };
        return serviceBusPayload;
    }*/

    public static LogMessage GetLogMessage(IConfiguration configuration, LogLevel severity, Guid logHandleId, string message )
    {
        ServiceSettings serviceSettings = configuration.GetSection(nameof(ServiceSettings)).Get<ServiceSettings>() ?? throw new InvalidOperationException("ServiceSettings is null");
        return new LogMessage
        {
            Id = Guid.NewGuid(),
            CreationDate = DateTimeOffset.Now,
            LogHandleId = logHandleId,
            /*Descriptor = $"{DateTimeOffset.Now} | {serviceSettings.ServiceName} | {message}",*/
            Descriptor = $"{serviceSettings.ServiceName} | {message}",
            Severity = severity
        };
    }
    
    public static LogMessage AttachLogMessage(this LogHandle logHandle, IConfiguration configuration, LogLevel severity, string message )
    {
        ServiceSettings serviceSettings = configuration.GetSection(nameof(ServiceSettings)).Get<ServiceSettings>() ?? throw new InvalidOperationException("ServiceSettings is null");
        var logMessage = new LogMessage
        {
            Id = Guid.NewGuid(),
            CreationDate = DateTimeOffset.Now,
            LogHandleId = logHandle.Id,
            /*Descriptor = $"{DateTimeOffset.Now} | {serviceSettings.ServiceName} | {message}",*/
            Descriptor = $"{serviceSettings.ServiceName} | {message}",
            Severity = severity
        };
        if (logHandle.Messages != null) 
            logHandle.Messages.Add(logMessage);
        else
        {
            logHandle.Messages = new List<LogMessage>();
            logHandle.Messages.Add(logMessage);
        }

        return logMessage;
    }
    
    public static T GetGenericLogMessage<T>(IConfiguration configuration, LogLevel severity, Guid logHandleId, string message ) where T: ILogMessage, new()
    {
        ServiceSettings serviceSettings = configuration.GetSection(nameof(ServiceSettings)).Get<ServiceSettings>() ?? throw new InvalidOperationException("ServiceSettings is null");
        return new T
        {
            Id = Guid.NewGuid(),
            CreationDate = DateTimeOffset.Now,
            LogHandleId = logHandleId,
            Descriptor = $"{serviceSettings.ServiceName} | {message}",
            Severity = severity
        };
    }
    
    /*public static ServiceBusPayload<TLogMessage> GetContract<TLogMessage>
        (IConfiguration configuration, LogLevel severity, Guid logHandleId, string message) where TLogMessage : ILogMessage, new()
    {
        var logMessage = GetGenericLogMessage<TLogMessage>(configuration, severity, logHandleId, message);
        var serviceBusPayload = new ServiceBusPayload<TLogMessage>
        {
            Subject = logMessage,
            Contract = nameof(CreateLogMessage),
            Descriptor = $"Creation of LogMessage {message}"
        };
        return serviceBusPayload;
    }*/
    
    public static void PublishLogMessage
        (this IPublishEndpoint publishEndpoint, IConfiguration configuration, LogLevel severity, Guid logHandleId, string message) 
    {
        var logMessage = GetLogMessage(configuration, severity, logHandleId, message);
        // var serviceBusPayload = new ServiceBusPayload<LogMessage>
        // {
        //     Subject = logMessage,
        //     Contract = nameof(CreateLogMessage),
        //     Descriptor = $"Creation of LogMessage {logMessage.Id}"
        // };
        publishEndpoint.Publish(new CreateLogMessage(logMessage));
    }


}