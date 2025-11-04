using AfricanNationsLeague.Infrastructure.Configuration;
using AfricanNationsLeague.Infrastructure.Interface;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AfricanNationsLeague.Infrastructure.Context
{
    public class MongoContext : IMongoContext
    {
        private readonly IMongoDatabase _database;
        private readonly MongoClient _client;

        public MongoContext(IOptions<MongoSettings> settings)
        {
            if (settings == null || settings.Value == null)
                throw new ArgumentNullException(nameof(settings), "MongoDB settings are missing.");

            _client = new MongoClient(settings.Value.ConnectionString);
            _database = _client.GetDatabase(settings.Value.Database);
        }

        public IMongoCollection<T> GetCollection<T>(string name)
        {
            return _database.GetCollection<T>(name);
        }

        public IMongoDatabase Database => _database;
    }
}
