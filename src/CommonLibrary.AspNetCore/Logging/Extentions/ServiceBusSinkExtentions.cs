using MassTransit;
using Serilog;
using Serilog.Configuration;

namespace CommonLibrary.AspNetCore.Logging;

public static class SinkExtentions
{
    public static LoggerConfiguration ServiceBusSink(
        this LoggerSinkConfiguration loggerConfiguration,
        IFormatProvider formatProvider = null, IPublishEndpoint publishEndpoint = null)
    {
        return loggerConfiguration.Sink(new ServiceBusSink(formatProvider, publishEndpoint));
    }
}