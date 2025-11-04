using AfricanNationsLeague.Domain.Entities;
using AfricanNationsLeague.Infrastructure.Interface;
using MongoDB.Driver;

namespace AfricanNationsLeague.Infrastructure.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly IMongoCollection<User> _collection;

        public UserRepository(IMongoContext context)
        {
            _collection = context.GetCollection<User>("users");
        }

        public async Task<User> AddAsync(User user)
        {
            await _collection.InsertOneAsync(user);
            return user;
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _collection.Find(u => u.Email == email).FirstOrDefaultAsync();
        }

        public async Task<User?> GetByIdAsync(string id)
        {
            return await _collection.Find(u => u.Id == id).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await _collection.Find(_ => true).ToListAsync();
        }

        public async Task DeleteAsync(string id)
        {
            await _collection.DeleteOneAsync(u => u.Id == id);
        }
    }
}
