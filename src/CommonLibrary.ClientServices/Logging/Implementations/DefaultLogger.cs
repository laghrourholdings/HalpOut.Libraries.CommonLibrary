using CommonLibrary.ClientServices.Logging.Interfaces;
using ILogger = Serilog.ILogger;

namespace CommonLibrary.ClientServices.Logging.Implementations;

public class DefaultLogger : IClientLogger
{
    private readonly ILogger _logger;

    public DefaultLogger(ILogger logger)
    {
        _logger = logger;
    }
    public void Debug(string message)
    {
        _logger.Debug("{@Message}",message);
    }

    public void Warning(string message)
    {
        _logger.Warning("{@Message}",message);
    }

    public void Error(string message)
    {
        _logger.Error("{@Message}",message);
    }

    public void Verbose(string message)
    {
        _logger.Verbose("{@Message}",message);
    }

    public void Information(string message)
    {
        _logger.Information("{@Message}",message);
    }
}