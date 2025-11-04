using AfricanNationsLeague.Domain.Entities;
using AfricanNationsLeague.Infrastructure.Interface;
using MongoDB.Driver;

namespace AfricanNationsLeague.Infrastructure.Repository
{
    public class CountryRepository : ICountryRepository
    {
        private readonly IMongoCollection<Country> _collection;

        public CountryRepository(IMongoDatabase database)
        {
            _collection = database.GetCollection<Country>("Countries");
        }

        public Task<List<Country>> GetAllAsync() =>
            _collection.Find(_ => true).ToListAsync();



        public Task AddManyAsync(List<Country> countries) =>
            _collection.InsertManyAsync(countries);

        public async Task<bool> AnyAsync() =>
            await _collection.CountDocumentsAsync(_ => true) > 0;
    }
}
