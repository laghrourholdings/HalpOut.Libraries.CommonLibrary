using System.ComponentModel.DataAnnotations.Schema;
using CommonLibrary.Core;
using Microsoft.Extensions.Logging;

namespace CommonLibrary.Logging;

 /// <summary>
    /// Default implementation for the ILogMessage BOI
/// </summary>
[Table("LogMessages")]
public class LogMessage : ILogMessage
{
    public Guid Id { get; set; }
    public DateTimeOffset CreationDate { get; set; }
    public bool IsDeleted { get; set; }
    public DateTimeOffset DeletedDate { get; set; }
    public bool IsSuspended { get; set; }
    public DateTimeOffset SuspendedDate { get; set; }
    
    public Guid LogHandleId { get; set; }
    public LogHandle LogHandle { get; set; } // Reference navigation
    
    public string? Descriptor { get; set; }
    public LogLevel Severity { get; set; }
}
