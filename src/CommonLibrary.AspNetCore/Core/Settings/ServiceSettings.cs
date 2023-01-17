using CommonLibrary.Logging;

namespace CommonLibrary.AspNetCore.Core;

 /// <summary>
    /// ServiceSettings provides utility and additionnal services data.
/// </summary>
public class ServiceSettings
{
    public string ServiceName { get; init; }
    public static string GetMessage(ref LoggingInterpolatedStringHandler handler) => handler.ToString();
}
