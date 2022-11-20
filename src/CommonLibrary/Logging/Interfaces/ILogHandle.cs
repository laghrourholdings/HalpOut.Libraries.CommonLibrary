using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CommonLibrary.Core;

namespace CommonLibrary.Logging;

public interface ILogHandle<TLogMessage,TEnumerable> : IObject, ISuspendable, IDeletable
    where TLogMessage : ILogMessage
    where TEnumerable:IEnumerable<TLogMessage>
{
    public Guid Id { get; set; }
    public string ObjectType { get; set; }
    public string? LocationDetails { get; set; }
    public string? AuthorizationDetails { get; set; }
    public TEnumerable Messages { get; set; }
}