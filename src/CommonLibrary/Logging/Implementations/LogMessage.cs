using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.Extensions.Logging;

namespace CommonLibrary.Logging;

public class LogMessage : ILogMessage
{
    public int Id { get; set; }
    public DateTimeOffset CreationDate { get; set; }
    public string Message { get; set; }
    public LogLevel Severity { get; set; }
}