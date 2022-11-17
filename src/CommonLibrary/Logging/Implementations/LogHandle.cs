using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CommonLibrary.Core;

namespace CommonLibrary.Logging;

/// <summary>
/// The LogHandle class is the default implementation of the ILogHandle
/// The LogMessage is used as the type parameter for ILogMessage
/// </summary>

public class LogHandle : ILogHandle<LogMessage,List<LogMessage>>
{
    [Key]
    public Guid Id { get; set; }
    public Guid ObjectId { get; set; }
    public DateTimeOffset CreationDate { get; set; }
    public bool IsDeleted { get; set; }
    public DateTimeOffset DeletedDate { get; set; }
    public bool IsSuspended { get; set; }
    public DateTimeOffset SuspendedDate { get; set; }
    public string? Descriptor { get; set; }

    public string ObjectType { get; set; }
    public string? LocationDetails { get; set; }
    public string? AuthorizationDetails { get; set; }
    public List<LogMessage>? Messages { get; set; }
}
