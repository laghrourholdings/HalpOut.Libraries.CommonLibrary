using MassTransit;
using Serilog.Core;
using Serilog.Events;

namespace CommonLibrary.AspNetCore.Logging;

public class ServiceBusSink : ILogEventSink
{
    private readonly IFormatProvider _formatProvider;
    private readonly IPublishEndpoint _publishEndpoint;
    public ServiceBusSink(IFormatProvider formatProvider, IPublishEndpoint publishEndpoint)
    {
        _formatProvider = formatProvider;
        _publishEndpoint = publishEndpoint;
    }
    
    //TODO: Creating the log model in the LogService as to create an ObjectMessage
    //Change log object so that message => json text
    public void Emit(LogEvent logEvent)
    {
        string servicename = logEvent.Properties.GetValueOrDefault("servicename")?.ToString()
                             ?? throw new InvalidOperationException();
        var logInput = logEvent.RenderMessage(_formatProvider);
        // var suggestedGuid = Guid.NewGuid();
        // var request = new ServiceBusRequest<Guid>
        // {
        //     Subject = suggestedGuid,
        //     Descriptor = $"Requesting object creation with guid: {suggestedGuid}",
        //     Contract = nameof(CreateObject)
        // };
        // Console.WriteLine($"Requesting object creation with guid: {suggestedGuid}");
        // await _publishEndpoint.Publish(new CreateObject(request));
        Console.WriteLine($"{logInput} with {servicename} received!!");
    }

}
