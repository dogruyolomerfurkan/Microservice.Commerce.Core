using System.Linq.Expressions;
using MongoDB.Driver;

namespace Commerce.Core.MongoDB;

public class MongoRepository<T> : IRepository<T> where T : IEntity
{
    private readonly IMongoCollection<T> _mongoCollection;
    private readonly FilterDefinitionBuilder<T> _filterBuilder = Builders<T>.Filter;

    public MongoRepository(IMongoDatabase mongoDatabase, string collection)
    {
        _mongoCollection = mongoDatabase.GetCollection<T>(collection);
    }

    public async Task<T> GetAsync(Guid id)
    {
        FilterDefinition<T> filter = _filterBuilder.Eq(entity => entity.Id, id);
        return await _mongoCollection.Find(filter).FirstOrDefaultAsync();
    }

    public async Task<T> GetAsync(Expression<Func<T, bool>> filter)
    {
        return await _mongoCollection.Find(filter).FirstOrDefaultAsync();
    }

    public async Task<IReadOnlyCollection<T>> GetListAsync()
    {
        return await _mongoCollection.Find(_filterBuilder.Empty).ToListAsync();
    }

    public async Task<IReadOnlyCollection<T>> GetListAsync(Expression<Func<T, bool>> filter)
    {
        return await _mongoCollection.Find(filter).ToListAsync();
    }

    public async Task CreateAsync(T entity)
    {
        if (entity is null) throw new ArgumentNullException(nameof(entity));
        await _mongoCollection.InsertOneAsync(entity);
    }

    public async Task UpdateAsync(T entity)
    {
        if (entity is null) throw new ArgumentNullException(nameof(entity));
        FilterDefinition<T> filter = _filterBuilder.Eq(existingEntity => existingEntity.Id, entity.Id);
        await _mongoCollection.ReplaceOneAsync(filter, entity);
    }

    public async Task RemoveAsync(Guid id)
    {
        FilterDefinition<T> filter = _filterBuilder.Eq(existingEntity => existingEntity.Id, id);
        await _mongoCollection.DeleteOneAsync(filter);
    }
}