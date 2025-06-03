using Backend_tidsregning.Core.Entities.DTO;
using Backend_tidsregning.Core.Entities.MongoDb;
using MongoDB.Driver;

namespace Backend_tidsregning.Core.Interfaces.Services;


public interface ICollectionService<T>
{
    Task<T> FindAsync(Guid id);
    Task<bool> TryAddAsync(T obj);

    Task<bool> TryRemoveAsync(Guid id);

    Task<bool> TryUpdateAsync(T obj);

    Task<List<T>> GetAllAsync();

    Task<long> GetCountAsync();
}