﻿using CommonLibrary.Core;

namespace CommonLibrary.Logging;

public interface ILogHandle<TLogMessage,TEnumerable> : IBusinessObject, ISuspendable, IDeletable
    where TLogMessage : ILogMessage
    where TEnumerable:IEnumerable<TLogMessage>
{
    public Guid LogHandleId { get; set; }
    public string ObjectType { get; set; }
    public string? LocationDetails { get; set; }
    public string? AuthorizationDetails { get; set; }
    public TEnumerable Messages { get; set; }
}