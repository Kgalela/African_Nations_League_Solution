using AfricanNationsLeague.Domain.Entities;
using AfricanNationsLeague.Infrastructure.Interface;
using MongoDB.Driver;

namespace AfricanNationsLeague.Infrastructure.Repository
{
    public class MatchRepository : IMatchRepository
    {
        private readonly IMongoCollection<Match> _collection;

        public MatchRepository(IMongoContext context)
        {
            _collection = context.GetCollection<Match>("matches");
        }

        public async Task<Match> AddAsync(Match match, CancellationToken ct = default)
        {
            await _collection.InsertOneAsync(match, cancellationToken: ct);
            return match;
        }

        public async Task<IEnumerable<Match>> GetAllAsync(CancellationToken ct = default)
            => await _collection.Find(_ => true).ToListAsync(ct);

        public async Task<Match?> GetByIdAsync(string id, CancellationToken ct = default)
            => await _collection.Find(m => m.Id == id).FirstOrDefaultAsync(ct);

        public async Task UpdateAsync(Match match, CancellationToken ct = default)
            => await _collection.ReplaceOneAsync(m => m.Id == match.Id, match, cancellationToken: ct);

        public async Task DeleteAsync(string id, CancellationToken ct = default)
            => await _collection.DeleteOneAsync(m => m.Id == id, ct);
    }
}
