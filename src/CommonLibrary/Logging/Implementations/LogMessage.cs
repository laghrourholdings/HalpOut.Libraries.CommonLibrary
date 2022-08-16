using System.ComponentModel.DataAnnotations.Schema;
using CommonLibrary.Core;
using Microsoft.Extensions.Logging;

namespace CommonLibrary.Logging;

 /// <summary>
    /// Default implementation for the ILogMessage BOI
/// </summary>
[Table("ILogMessage")]
public class LogMessage : ILogMessage
{
    public int Id { get; set; }
    public Guid ObjectId { get; set; }
    public IIObject Object { get; set; }
    public DateTimeOffset CreationDate { get; set; }
    public string Message { get; set; }
    public LogLevel Severity { get; set; }
}
