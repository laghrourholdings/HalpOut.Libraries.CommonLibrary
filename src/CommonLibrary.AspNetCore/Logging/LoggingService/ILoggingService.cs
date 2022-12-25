namespace CommonLibrary.AspNetCore.Logging.LoggingService;

public interface ILoggingService
{
    public void InformationToLogService(
        string message,
        Guid logHandleId);
    
    public void VerboseToLogService(
        string message,
        Guid logHandleId);
    
    public void FatalToLogService(
        string message,
        Guid logHandleId);
    
    
    public void WarningToLogService(
        string message,
        Guid logHandleId);
    
    public void ErrorToLogService(
        string message,
        Guid logHandleId);
    
    public void DebugToLogService(
        string message,
        Guid logHandleId);
    
    public Serilog.ILogger Log();
}