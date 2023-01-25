using CommonLibrary.Core;
using Microsoft.Extensions.Logging;

namespace CommonLibrary.Logging;

public interface ILogMessage :  IBusinessObject
{ 
    public LogLevel Severity { get; set; }
}