using Microsoft.Extensions.Logging;

namespace CommonLibrary.Logging;

public interface ILogMessage

{
    public Int32 Id { get; set; }
    
    public DateTimeOffset CreationDate { get; set; }
    public string Message { get; set; }
    public LogLevel Severity { get; set; }
}