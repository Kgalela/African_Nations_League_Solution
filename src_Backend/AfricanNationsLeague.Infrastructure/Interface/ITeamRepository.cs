using AfricanNationsLeague.Domain.Entities;

namespace AfricanNationsLeague.Infrastructure.Interface
{
    public interface ITeamRepository
    {
        Task<Team> AddAsync(Team team);
        Task<IEnumerable<Team>> GetAllAsync();
        Task<Team?> GetByIdAsync(string id);
        Task DeleteAsync(string id);
    }
}
