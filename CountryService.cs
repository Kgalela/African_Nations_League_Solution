using AfricanNationsLeague.Application.Models;
using AfricanNationsLeague.Infrastructure.Interface;

namespace AfricanNationsLeague.Application.Services
{
    public class CountryService
    {
        private readonly ICountryRepository _countryrepo;

        public CountryService(ICountryRepository country)
        {
            _countryrepo = country;
        }

        public async Task<List<CountriesDto>> GetAllAsync()
        {
            var countries = await _countryrepo.GetAllAsync().ConfigureAwait(false);
            return countries?.Select(c => new CountriesDto
            {
                Code = c.Code,
                Name = c.Name,
                FlagUrl = c.FlagUrl
            }).ToList() ?? new List<CountriesDto>();
        }
    }
}
