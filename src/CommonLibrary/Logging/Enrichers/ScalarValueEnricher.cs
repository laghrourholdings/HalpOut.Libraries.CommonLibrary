using Serilog.Core;
using Serilog.Events;

namespace CommonLibrary.Logging;

public class ScalarValueEnricher : ILogEventEnricher
{
    readonly LogEventProperty _prop;

    public ScalarValueEnricher(string name, object value)
    {
        _prop = new LogEventProperty(name, new ScalarValue(value));
    }

    public void Enrich(LogEvent evt, ILogEventPropertyFactory _)
    {
        evt.AddPropertyIfAbsent(_prop);
    }
}