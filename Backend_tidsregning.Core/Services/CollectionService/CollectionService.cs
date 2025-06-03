using System.Runtime.CompilerServices;
using Backend_tidsregning.Core.Context;
using Backend_tidsregning.Core.Interfaces.Entites;
using Backend_tidsregning.Core.Interfaces.Services;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Backend_tidsregning.Core.Services.CollectionService;


public class CollectionService<T> : ICollectionService<T>
where T: IMongoDbId
{
    private IMongoCollection<T> _collection;
    public CollectionService(IOptions<ServiceOptions.ServiceOptions> options, MongoDbContext context)
    {
        _collection = context.Database.GetCollection<T>(options.Value.Collection);
     }
    public async Task<T> FindAsync(Guid id)
    {
        var filter = Builders<T>.Filter.Eq(obj => obj._ID, id);
        var result = await _collection.FindAsync(filter);
        return await result.FirstOrDefaultAsync();
    }

    public async Task<List<T>> GetAllAsync()
    {
        var documents = await _collection.FindAsync(Builders<T>.Filter.Empty);
        return await documents.ToListAsync();
    }

    public async Task<long> GetCountAsync()
    {
        return await _collection.CountDocumentsAsync(Builders<T>.Filter.Empty);
    }

    public async Task<bool> TryAddAsync(T obj)
    {
        try
        {
            await _collection.InsertOneAsync(obj);
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public async Task<bool> TryRemoveAsync(Guid id)
    {
        try
        {
            var filter = Builders<T>.Filter.Eq(obj => obj._ID, id);
            await _collection.FindOneAndDeleteAsync(filter);
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public async Task<bool> TryUpdateAsync(T obj)
    {
        try
        {
            var filter = Builders<T>.Filter.Eq(elem => elem._ID, obj._ID);
            await _collection.FindOneAndReplaceAsync(filter, obj);
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }
}
