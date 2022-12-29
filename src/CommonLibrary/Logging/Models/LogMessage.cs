using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.Extensions.Logging;

namespace CommonLibrary.Logging.Models;

 /// <summary>
    /// Default implementation for the ILogMessage BOI
/// </summary>
[Table("LogMessages")]
public class LogMessage : ILogMessage, IEquatable<LogMessage>
{
    public Guid Id { get; set; }
    public Guid LogHandleId { get; set; }
    public DateTimeOffset CreationDate { get; set; }
    public LogLevel Severity { get; set; }
    public string? Descriptor { get; set; }


    public bool Equals(LogMessage? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return Id.Equals(other.Id) && LogHandleId.Equals(other.LogHandleId) && CreationDate.Equals(other.CreationDate) && Severity == other.Severity && Descriptor == other.Descriptor;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((LogMessage)obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Id, LogHandleId, CreationDate, (int)Severity, Descriptor);
    }
}
