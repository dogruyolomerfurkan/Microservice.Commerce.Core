using Commerce.Core.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;

namespace Commerce.Core.MongoDB;

public static class Extensions
{
    public static IServiceCollection AddMongoDB(this IServiceCollection services)
    {
        BsonSerializer.RegisterSerializer(new GuidSerializer(BsonType.String));
        BsonSerializer.RegisterSerializer(new DateTimeOffsetSerializer(BsonType.String));

        services.AddSingleton(serviceProvider =>
        {
            var configuration = serviceProvider.GetService<IConfiguration>();
            var mongoDBSettings = configuration?.GetSection(nameof(MongoDBSettings)).Get<MongoDBSettings>();
            var serviceSettings = configuration?.GetSection(nameof(ServiceSettings)).Get<ServiceSettings>();
            var mongoClient = new MongoClient(mongoDBSettings?.ConnectionString);
            return mongoClient.GetDatabase(serviceSettings?.Name);
        });

        return services;
    }

    public static IServiceCollection AddMongoDBRepository<T>(this IServiceCollection services, string collection)
    where T : IEntity
    {
        services.AddSingleton<IRepository<T>>(serviceProvider =>
        {
            var database = serviceProvider.GetService<IMongoDatabase>();
            return new MongoRepository<T>(database, collection);
        });

        return services;
    }
}