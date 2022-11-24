using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.Extensions.Logging;

namespace CommonLibrary.Logging;

 /// <summary>
    /// Default implementation for the ILogMessage BOI
/// </summary>
[Table("LogMessages")]
public class LogMessage : ILogMessage
{
    public Guid Id { get; set; }
    public Guid LogHandleId { get; set; }
    public DateTimeOffset CreationDate { get; set; }
    public LogLevel Severity { get; set; }
    public string? Descriptor { get; set; }
}
