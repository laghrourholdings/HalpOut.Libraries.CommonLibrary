using CommonLibrary.Core;

namespace CommonLibrary.Logging;

/// <summary>
/// The LogHandle class is the default implementation of the ILogHandle
/// The LogMessage is used as the type parameter for ILogMessage
/// </summary>
public class LogHandle : ILogHandle<LogMessage,ICollection<LogMessage>/*,IIObject*/>
{
    public Guid Id { get; set; }
    public Guid ObjectId { get; set; }
    //public IIObject? Object { get; set; }
    
    public Guid? SubjectId { get; set; }
    public string? LocationDetails { get; set; }
    public string? AuthorizationDetails { get; set; }
    public ICollection<LogMessage> Messages { get; set; } = new List<LogMessage>();
}
