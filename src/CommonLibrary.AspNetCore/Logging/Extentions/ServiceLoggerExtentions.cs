using CommonLibrary.AspNetCore.ServiceBus;
using CommonLibrary.Logging;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using ILogger = Serilog.ILogger;

namespace CommonLibrary.AspNetCore.Logging;

public static class ServiceLoggerExtentions
{

    
    // public static void ScanToBusLog<T>(IServiceBusLogContext<T> context,
    //     ILogger logger, ref LoggingInterpolatedStringHandler handler)
    //     where T : IServiceBusPayload<T>
    // {
    //     
    // }
    
    public static void InformationToBusLog(this ILogger logger, IConfiguration config,
          string message, Guid logHandleId, IPublishEndpoint publishEndpoint)
    {
        logger.Information(message);
        publishEndpoint.PublishLogMessage(config, LogLevel.Information, logHandleId,message);
    }
    

    public static void VerboseToBusLog(this ILogger logger, IConfiguration config,
        string message, Guid logHandleId, IPublishEndpoint publishEndpoint)
    {
        logger.Information(message);
        publishEndpoint.PublishLogMessage(config, LogLevel.None, logHandleId, message);
    }

    public static void FatalToBusLog(this ILogger logger, IConfiguration config,
        string message, Guid logHandleId, IPublishEndpoint publishEndpoint)
    {
        logger.Information(message);
        publishEndpoint.PublishLogMessage(config, LogLevel.Critical, logHandleId, message);
    }

    public static void WarningToBusLog(this ILogger logger, IConfiguration config,
        string message, Guid logHandleId, IPublishEndpoint publishEndpoint)
    {
        logger.Information(message);
        publishEndpoint.PublishLogMessage(config, LogLevel.Warning, logHandleId, message);
    }
    
    public static void ErrorToBusLog(this ILogger logger, IConfiguration config,
        string message, Guid logHandleId, IPublishEndpoint publishEndpoint)
    {
        logger.Information(message);
        publishEndpoint.PublishLogMessage(config, LogLevel.Error, logHandleId, message);
    }
    
    public static void DebugToBusLog(this ILogger logger, IConfiguration config,
        string message, Guid logHandleId, IPublishEndpoint publishEndpoint)
    {
        logger.Information(message);
        publishEndpoint.PublishLogMessage(config, LogLevel.Debug, logHandleId, message);
    }

    /*public static void CriticalBusLog<T>(this ILogger logger, IServiceBusLogContext<T> context, 
        ref LoggingInterpolatedStringHandler handler, IPublishEndpoint publishEndpoint, object logContract)
        where T : IServiceBusMessage
    {
        logger.Critical(ref handler);
        publishEndpoint.Publish(logContract);
    }
    
    public static void General(this ILogger logger, ref LoggingInterpolatedStringHandler handler)
    {
        logger.Information(handler.ToString(), handler.Parameters.ToArray());
    }
    public static void Warn(this ILogger logger, ref LoggingInterpolatedStringHandler handler)
    {
        logger.Warning(handler.ToString(), handler.Parameters.ToArray());
    }
    public static void Meta(this ILogger logger, ref LoggingInterpolatedStringHandler handler)
    {
        logger.Verbose(handler.ToString(), handler.Parameters.ToArray());
    }
    public static void Scan(this ILogger logger, ref LoggingInterpolatedStringHandler handler)
    {
        logger.Debug(handler.ToString(), handler.Parameters.ToArray());
    }
    public static void Critical(this ILogger logger, ref LoggingInterpolatedStringHandler handler)
    {
        logger.Fatal(handler.ToString(), handler.Parameters.ToArray());
    }
    public static void Failure(this ILogger logger, ref LoggingInterpolatedStringHandler handler)
    {
        logger.Error(handler.ToString(), handler.Parameters.ToArray());
    }*/
}