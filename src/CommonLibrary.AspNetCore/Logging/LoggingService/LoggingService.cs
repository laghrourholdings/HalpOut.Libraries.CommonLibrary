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
    public void DebugToLogService(string message, Guid logHandleId)
    {
        _logger.Debug(message);
        _logger.DebugToBusLog(_config,message, logHandleId, _publishEndpoint);
    }

    public void InformationToLogService(string message, Guid logHandleId)
    {
        _logger.Information(message);
        _logger.InformationToBusLog(_config,message, logHandleId, _publishEndpoint);
    }

    public void VerboseToLogService(string message, Guid logHandleId)
    {
        _logger.Verbose(message);
        _logger.VerboseToBusLog(_config,message, logHandleId, _publishEndpoint);
    }

    public void FatalToLogService(string message, Guid logHandleId)
    {
        _logger.Fatal(message);
        _logger.FatalToBusLog(_config,message, logHandleId, _publishEndpoint);
    }

    public void WarningToLogService(string message, Guid logHandleId)
    {
        _logger.Warning(message);
        _logger.WarningToBusLog(_config,message, logHandleId, _publishEndpoint);
    }

    public void ErrorToLogService(string message, Guid logHandleId)
    {
        _logger.Error(message);
        _logger.ErrorToBusLog(_config,message, logHandleId, _publishEndpoint);
    }
}