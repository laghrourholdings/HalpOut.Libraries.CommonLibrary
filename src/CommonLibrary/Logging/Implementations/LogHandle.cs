using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CommonLibrary.Core;

namespace CommonLibrary.Logging;

/// <summary>
/// The LogHandle class is the default implementation of the ILogHandle
/// The LogMessage is used as the type parameter for ILogMessage
/// </summary>

public class LogHandle : ILogHandle<LogMessage,ICollection<LogMessage>,IIObject>
{
    [Key]
    public Guid Id { get; set; }
    public Guid ObjectId { get; set; }
    [ForeignKey("ObjectId")]
    public IIObject Object { get; set; }
    public string? LocationDetails { get; set; }
    public string? AuthorizationDetails { get; set; }
    public ICollection<LogMessage> Messages { get; set; } = new List<LogMessage>();
}
