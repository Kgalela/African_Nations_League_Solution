using AfricanNationsLeague.Domain.Entities;

namespace AfricanNationsLeague.Infrastructure.Interface
{
    public interface ICountryRepository
    {
        Task<List<Country>> GetAllAsync();


        Task AddManyAsync(List<Country> countries);
        Task<bool> AnyAsync();
    }
}
