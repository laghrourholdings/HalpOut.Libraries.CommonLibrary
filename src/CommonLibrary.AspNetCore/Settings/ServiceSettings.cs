using CommonLibrary.Logging;

namespace CommonLibrary.AspNetCore.Settings;

public class ServiceSettings
{
    public string ServiceName { get; init; }
    public static string GetMessage(ref LoggingInterpolatedStringHandler handler) => handler.ToString();

}