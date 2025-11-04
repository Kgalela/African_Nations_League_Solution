using AfricanNationsLeague.Domain.Entities;
using AfricanNationsLeague.Infrastructure.Interface;
using MongoDB.Driver;

namespace AfricanNationsLeague.Infrastructure.Repository
{
    public class TeamRepository : ITeamRepository
    {
        private readonly IMongoCollection<Team> _collection;

        public TeamRepository(IMongoContext context)
        {
            _collection = context.GetCollection<Team>("teams");
        }

        //public async Task<Team> AddAsync(Team team)
        //{
        //    await _collection.InsertOneAsync(team);
        //    return team;
        //}
        public async Task<Team> AddAsync(Team team)
        {
            await _collection.InsertOneAsync(team);
            return await GetByIdAsync(team.Id); // Ensures you get the Id from DB
        }



        public async Task<IEnumerable<Team>> GetAllAsync()
        {
            return await _collection.Find(_ => true).ToListAsync();
        }

        public async Task<Team?> GetByIdAsync(string id)
        {
            return await _collection.Find(t => t.Id == id).FirstOrDefaultAsync();
        }

        public async Task DeleteAsync(string id)
        {
            await _collection.DeleteOneAsync(t => t.Id == id);
        }
    }
}
