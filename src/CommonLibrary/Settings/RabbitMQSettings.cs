namespace CommonLibrary.Settings;

public class RabbitMQSettings
{
    public string Host { get; init; }
    public ushort Port { get; init; }
    public string ConnectionString => $"rabbitmq://{Host}:{Port}";
}