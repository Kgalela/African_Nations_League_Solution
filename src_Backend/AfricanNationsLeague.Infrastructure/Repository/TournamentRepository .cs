using AfricanNationsLeague.Domain.Entities;
using AfricanNationsLeague.Infrastructure.Interface;
using MongoDB.Driver;

namespace AfricanNationsLeague.Infrastructure.Repository
{
    public class TournamentRepository : ITournamentRepository
    {
        private readonly IMongoCollection<Tournament> _collection;

        public TournamentRepository(IMongoContext context)
        {
            _collection = context.GetCollection<Tournament>("tournaments");
        }

        public async Task<Tournament> AddAsync(Tournament tournament, CancellationToken ct = default)
        {
            await _collection.InsertOneAsync(tournament, cancellationToken: ct);
            return tournament;
        }

        public async Task<IEnumerable<Tournament>> GetAllAsync(CancellationToken ct = default)
        {
            return await _collection.Find(_ => true).ToListAsync(ct);
        }

        public async Task<Tournament?> GetByIdAsync(string id, CancellationToken ct = default)
        {
            return await _collection.Find(t => t.Id == id).FirstOrDefaultAsync(ct);
        }

        public async Task UpdateAsync(Tournament tournament, CancellationToken ct = default)
        {
            await _collection.ReplaceOneAsync(t => t.Id == tournament.Id, tournament, cancellationToken: ct);
        }

        public async Task DeleteAsync(string id, CancellationToken ct = default)
        {
            await _collection.DeleteOneAsync(t => t.Id == id, cancellationToken: ct);
        }
    }
}
