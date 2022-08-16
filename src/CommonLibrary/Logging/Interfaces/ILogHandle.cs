using CommonLibrary.Core;

namespace CommonLibrary.Logging;

public interface ILogHandle<TLogMessage,TEnumerable/*,TObject*/> 
    where TLogMessage : ILogMessage
    where TEnumerable:IEnumerable<TLogMessage>
    /*where TObject : IObject*/
{
    public Guid Id { get; set; }
    public Guid ObjectId { get; set; }
    //public TObject? Object{ get; set; }
    
    public Guid? SubjectId { get; set; }
    public string? LocationDetails { get; set; }
    public string? AuthorizationDetails { get; set; }
    public TEnumerable Messages { get; set; }
}