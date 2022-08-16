using CommonLibrary.Core;

namespace CommonLibrary.Logging;

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