using AfricanNationsLeague.Domain.Entities;

namespace AfricanNationsLeague.Infrastructure.Interface
{
    public interface IMatchRepository
    {
        Task<Match> AddAsync(Match match, CancellationToken ct = default);
        Task<IEnumerable<Match>> GetAllAsync(CancellationToken ct = default);
        Task<Match?> GetByIdAsync(string id, CancellationToken ct = default);
        Task UpdateAsync(Match match, CancellationToken ct = default);
        Task DeleteAsync(string id, CancellationToken ct = default);
    }
}
