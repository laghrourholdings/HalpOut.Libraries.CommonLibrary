using System.Runtime.CompilerServices;

namespace CommonLibrary.Logging;

[InterpolatedStringHandler]
public ref struct LoggingInterpolatedStringHandler
{

    private DefaultInterpolatedStringHandler _innerHandler;
    public List<object?> Parameters { get; } = new();

    public LoggingInterpolatedStringHandler(int literalLength, int formattedCount)
    {
        _innerHandler = new DefaultInterpolatedStringHandler(literalLength, formattedCount);
    }
    public void AppendLiteral(string literal)
    {
        _innerHandler.AppendLiteral(literal);
    }

    public void AppendFormatted<T>(T message,
        [CallerArgumentExpression("message")] string callerName = "")
    {
        _innerHandler.AppendFormatted("{"+callerName+"}");
        Parameters.Add(message);
    }
    
    public override string ToString()
    {
        return _innerHandler.ToString();
    }

    public string ToStringAndClear()
    {
        return _innerHandler.ToStringAndClear();
    }
}