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
    public Serilog.ILogger Log()
    {
        return _logger;
    }
    //TODO Checks
    public void Debug(string message, Guid logHandleId = default)
    {
        _logger.Debug(message);
        if(logHandleId != Guid.Empty)
            _logger.DebugToBusLog(_config,message, logHandleId, _publishEndpoint);
    }

    public void Information(string message, Guid logHandleId)
    {
        _logger.Information(message);
        if(logHandleId != Guid.Empty)
            _logger.InformationToBusLog(_config,message, logHandleId, _publishEndpoint);
    }

    public void Verbose(string message, Guid logHandleId)
    {
        _logger.Verbose(message);
        if(logHandleId != Guid.Empty);
            _logger.VerboseToBusLog(_config,message, logHandleId, _publishEndpoint);
    }

    public void Critical(string message, Guid logHandleId)
    {
        _logger.Fatal(message);
        if(logHandleId != Guid.Empty)
            _logger.FatalToBusLog(_config,message, logHandleId, _publishEndpoint);
    }

    public void Warning(string message, Guid logHandleId)
    {
        _logger.Warning(message);
        if(logHandleId != Guid.Empty)
            _logger.WarningToBusLog(_config,message, logHandleId, _publishEndpoint);
    }

    public void Error(string message, Guid logHandleId)
    {
        _logger.Error(message);
        if(logHandleId != Guid.Empty)
            _logger.ErrorToBusLog(_config,message, logHandleId, _publishEndpoint);
    }
}