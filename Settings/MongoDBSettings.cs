namespace Commerce.Core.Settings;

public class MongoDBSettings
{
    public string Host { get; init; } = null!;
    public string Port { get; init; } = null!;
    public string ConnectionString => $"mongodb://{Host}:{Port}";
}