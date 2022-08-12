
namespace CommonLibrary.Logging;
using ILogger = Serilog.ILogger;

public static class LoggingExtentions
{
    public static void InformationF(this ILogger logger, ref LoggingInterpolatedStringHandler handler)
    {
        logger.Information(handler.ToString(), handler.Parameters.ToArray());
    }
}