namespace CommonLibrary.AspNetCore.Settings;

public class RabbitMQSettings
{
    public string Host { get; init; } = "localhost";
    public ushort Port { get; init; } = 5672;
    public string ConnectionString => $"rabbitmq://{Host}:{Port}";
}