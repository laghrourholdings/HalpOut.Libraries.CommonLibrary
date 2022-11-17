using CommonLibrary.AspNetCore.ServiceBus;
using CommonLibrary.Logging;
using MassTransit;
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
    
    public static void GeneralToBusLog<T>(this ILogger logger, IServiceBusLogContext<T> context,
         ref LoggingInterpolatedStringHandler handler, IPublishEndpoint publishEndpoint, object logContract)
        where T :IServiceBusMessage
    {
        logger.General(ref handler);
        publishEndpoint.Publish(logContract);
    }

    public static void MetaToBusLog<T>(this ILogger logger, IServiceBusLogContext<T> context,
        ref LoggingInterpolatedStringHandler handler, IPublishEndpoint publishEndpoint, object logContract)
        where T : IServiceBusMessage
    {
        logger.Meta(ref handler);
        publishEndpoint.Publish(logContract);
    }

    public static void FailureToBusLog<T>(this ILogger logger, IServiceBusLogContext<T> context,
        ref LoggingInterpolatedStringHandler handler, IPublishEndpoint publishEndpoint, object logContract)
        where T : IServiceBusMessage
    {
        logger.Failure(ref handler);
        publishEndpoint.Publish(logContract);
    }

    public static void WarnToBusLog<T>(
        this ILogger logger, IServiceBusLogContext<T> context, 
        ref LoggingInterpolatedStringHandler handler, IPublishEndpoint publishEndpoint, object logContract)
        where T : IServiceBusMessage
    {
        logger.Warn(ref handler);
        publishEndpoint.Publish(logContract);
    }
    
   

    public static void CriticalBusLog<T>(this ILogger logger, IServiceBusLogContext<T> context, 
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
    }
}