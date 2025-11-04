using AfricanNationsLeague.Domain.Entities;

namespace AfricanNationsLeague.Infrastructure.Interface
{
    public interface ITournamentRepository
    {
        Task<Tournament> AddAsync(Tournament tournament, CancellationToken ct = default);
        Task<IEnumerable<Tournament>> GetAllAsync(CancellationToken ct = default);
        Task<Tournament?> GetByIdAsync(string id, CancellationToken ct = default);
        Task UpdateAsync(Tournament tournament, CancellationToken ct = default);
        Task DeleteAsync(string id, CancellationToken ct = default);
    }
}
