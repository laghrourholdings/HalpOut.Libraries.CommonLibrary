using CommonLibrary.Core;
using Microsoft.Extensions.Logging;

namespace CommonLibrary.Logging;

public interface ILogMessage :  IObject, ILoggable

{ 
    public LogLevel Severity { get; set; }
}