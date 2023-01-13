namespace CommonLibrary.Logging.Models.Dtos;

/// <summary>
/// The LogHandle class is the default implementation of the ILogHandle
/// The LogMessage is used as the type parameter for ILogMessage
/// </summary>

public class LogHandleDto //: ILogHandle<LogMessage,List<LogMessage>>, IEquatable<LogHandle>
{
    public Guid Id { get; set; }
    public Guid ObjectId { get; set; }
    public DateTimeOffset CreationDate { get; set; }
    public string ObjectType { get; set; }
    public string? LocationDetails { get; set; }
    public string? AuthorizationDetails { get; set; }
    public string? Descriptor { get; set; }
    public ICollection<LogMessageDto> Messages { get; set; } = new HashSet<LogMessageDto>();
    public bool Equals(LogHandleDto? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return Id.Equals(other.Id) && ObjectId.Equals(other.ObjectId);
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((LogHandleDto)obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Id, ObjectId);
    }
}
