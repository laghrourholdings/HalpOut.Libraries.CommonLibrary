using Microsoft.Extensions.Logging;

namespace CommonLibrary.Logging.Models.Dtos;

 /// <summary>
    /// Default implementation for the ILogMessage BOI
/// </summary>
public class LogMessageDto //: ILogMessage, IEquatable<LogMessage>
{
    public Guid LogHandleId { get; set; }
    public DateTimeOffset CreationDate { get; set; }
    public LogLevel Severity { get; set; }
    public string Message { get; set; }

    public bool Equals(LogMessageDto? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return LogHandleId.Equals(other.LogHandleId) && CreationDate.Equals(other.CreationDate) && Severity == other.Severity && Message == other.Message;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((LogMessageDto)obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(LogHandleId, CreationDate, (int)Severity, Message);
    }
}
