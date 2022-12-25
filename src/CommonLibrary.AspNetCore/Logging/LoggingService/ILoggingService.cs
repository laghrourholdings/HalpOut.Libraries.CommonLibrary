namespace CommonLibrary.AspNetCore.Logging.LoggingService;

public interface ILoggingService
{
    public void Information(
        string message,
        Guid logHandleId = default);
    
    public void Verbose(
        string message,
        Guid logHandleId = default);
    
    public void Critical(
        string message,
        Guid logHandleId = default);
    
    
    public void Warning(
        string message,
        Guid logHandleId = default);
    
    public void Error(
        string message,
        Guid logHandleId = default);
    
    public void Debug(
        string message,
        Guid logHandleId = default);
    
    public Serilog.ILogger Log();
}