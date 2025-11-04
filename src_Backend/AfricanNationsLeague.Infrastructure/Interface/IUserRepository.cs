using AfricanNationsLeague.Domain.Entities;


namespace AfricanNationsLeague.Infrastructure.Interface
{
    public interface IUserRepository
    {
        Task<User> AddAsync(User user);
        Task<User?> GetByEmailAsync(string email);
        Task<User?> GetByIdAsync(string id);
        Task<IEnumerable<User>> GetAllAsync();
        Task DeleteAsync(string id);
    }
}
