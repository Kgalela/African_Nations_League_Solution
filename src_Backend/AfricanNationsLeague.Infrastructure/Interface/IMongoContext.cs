using MongoDB.Driver;

namespace AfricanNationsLeague.Infrastructure.Interface
{
    public interface IMongoContext
    {
        IMongoCollection<T> GetCollection<T>(string name);
        IMongoDatabase Database { get; }
    }
}
