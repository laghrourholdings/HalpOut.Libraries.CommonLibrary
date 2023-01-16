namespace CommonLibrary.AspNetCore.Logging;

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

    public void CreateLogHandle(Guid logHandleId, Guid targetObjectId, string objectType){}
    public Serilog.ILogger Local();
}