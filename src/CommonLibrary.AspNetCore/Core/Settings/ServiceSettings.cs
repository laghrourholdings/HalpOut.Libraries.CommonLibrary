namespace CommonLibrary.AspNetCore.Core;

 /// <summary>
    /// ServiceSettings provides utility and additionnal services data.
/// </summary>
public class ServiceSettings
{
    public string? ServiceName { get; init; }
    public string? SecuromanCacheConfiguration { get; init; }
    public string? UserServiceUrl { get; init; }
    public string? UserServiceGrpcUrl { get; init; }
    
    public MongoDbSettings? RolemanDatabase { get; init; }
    public RabbitMQSettings? MessageBus { get; init; }
    //public static string GetMessage(ref LoggingInterpolatedStringHandler handler) => handler.ToString();
}
