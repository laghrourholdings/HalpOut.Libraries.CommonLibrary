using CommonLibrary.AspNetCore.ServiceBus.Contracts.Logging;
using MassTransit;
using Microsoft.Extensions.Configuration;

namespace CommonLibrary.AspNetCore.Logging.LoggingService;

public class LoggingService : ILoggingService
{
    
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly IConfiguration _config;
    private readonly Serilog.ILogger _logger;
    
    public LoggingService(
        IPublishEndpoint publishEndpoint,
        IConfiguration config,
        Serilog.ILogger logger)
    {
        _publishEndpoint = publishEndpoint;
        _config = config;
        _logger = logger;
    }
    /// <summary>
    /// Returns the current ILogger
    /// </summary>
    /// <returns>The Serilog's ILogger instance.</returns>
    public Serilog.ILogger Local()
    {
        return _logger;
    }
    
    //TODO Checks with cache to see if the logHandleId is valid
    /// <summary>
    /// Logs a debug message to configured sinks, and sends a copy to the database for archiving if a logHandleId is provided.
    /// For debug types of messages, refrain from using the logHandleId in production. 
    /// </summary>
    /// <param name="message">Log message</param>
    /// <param name="logHandleId">Important: Must be provided if the message's target object has a logHandleId field</param>
    public void Debug(string message, Guid logHandleId = default)
    {
        _logger.Debug(message);
        if(logHandleId != Guid.Empty)
            _logger.DebugToBusLog(_config,message, logHandleId, _publishEndpoint);
    }

    /// <summary>
    /// Logs an information message to the configured sinks, and sends a copy to the database for archiving if a logHandleId is provided. 
    /// </summary>
    /// <param name="message">Log message</param>
    /// <param name="logHandleId">Important: Must be provided if the message's target object has a logHandleId field</param>
    public void Information(string message, Guid logHandleId = default)
    {
        _logger.Information(message);
        if(logHandleId != Guid.Empty)
            _logger.InformationToBusLog(_config,message, logHandleId, _publishEndpoint);
    }
    
    /// <summary>
    /// Logs a verbose message to the configured sinks, and sends a copy to the database for archiving if a logHandleId is provided.
    /// </summary>
    /// <param name="message">Log message</param>
    /// <param name="logHandleId">Important: Must be provided if the message's target object has a logHandleId field</param>
    public void Verbose(string message, Guid logHandleId = default)
    {
        _logger.Verbose(message);
        if(logHandleId != Guid.Empty);
            _logger.VerboseToBusLog(_config,message, logHandleId, _publishEndpoint);
    }
    
    /// <summary>
    /// Logs a critical message to the configured sinks, and sends a copy to the database for archiving if a logHandleId is provided.
    /// </summary>
    /// <param name="message">Log message</param>
    /// <param name="logHandleId">Important: Must be provided if the message's target object has a logHandleId field</param>
    public void Critical(string message, Guid logHandleId = default)
    {
        _logger.Fatal(message);
        if(logHandleId != Guid.Empty)
            _logger.FatalToBusLog(_config,message, logHandleId, _publishEndpoint);
    }
    
    /// <summary>
    /// Logs a warning message to the configured sinks, and sends a copy to the database for archiving if a logHandleId is provided.
    /// </summary>
    /// <param name="message">Log message</param>
    /// <param name="logHandleId">Important: Must be provided if the message's target object has a logHandleId field</param>
    public void Warning(string message, Guid logHandleId = default)
    {
        _logger.Warning(message);
        if(logHandleId != Guid.Empty)
            _logger.WarningToBusLog(_config,message, logHandleId, _publishEndpoint);
    }

    /// <summary>
    /// Logs an error message to the configured sinks, and sends a copy to the database for archiving if a logHandleId is provided.
    /// </summary>
    /// <param name="message">Log message</param>
    /// <param name="logHandleId">Important: Must be provided if the message's target object has a logHandleId field</param>
    public void Error(string message, Guid logHandleId = default)
    {
        _logger.Error(message);
        if(logHandleId != Guid.Empty)
            _logger.ErrorToBusLog(_config,message, logHandleId, _publishEndpoint);
    }
    
    /// <summary>
    /// Creates a new logHandleId for a given object.
    /// </summary>
    public void CreateLogHandle(Guid logHandleId, Guid targetObjectId, string objectType)
    { 
        _publishEndpoint.Publish(new CreateLogHandle(logHandleId, targetObjectId, objectType));
    }
}