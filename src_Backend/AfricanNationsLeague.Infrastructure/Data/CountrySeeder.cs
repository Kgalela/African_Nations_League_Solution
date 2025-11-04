using AfricanNationsLeague.Domain.Entities;
using MongoDB.Driver;
using System.Net.Http.Json;

namespace AfricanNationsLeague.Infrastructure.Data
{
    public class CountrySeeder
    {
        public static async Task SeedAsync(IMongoDatabase database, HttpClient http)
        {
            var collection = database.GetCollection<Country>("Countries");

            if (await collection.CountDocumentsAsync(_ => true) > 0)
                return;

            var apiData = await http.GetFromJsonAsync<List<RestCountry>>(
                "https://restcountries.com/v3.1/all?fields=cca2,name,flags");


            var countries = apiData
                .Where(c => c.Cca2 != null)
                .Select(c => new Country
                {
                    Code = c.Cca2.ToUpper(),
                    Name = c.Name.Common,
                    FlagUrl = c.Flags.Png
                })
                .OrderBy(c => c.Name)
                .ToList();

            await collection.InsertManyAsync(countries);
        }
    }

    public class RestCountry
    {
        public Name Name { get; set; }
        public Flags Flags { get; set; }
        public string Cca2 { get; set; }
    }

    public class Name
    {
        public string Common { get; set; }
    }

    public class Flags
    {
        public string Png { get; set; }
    }

}
