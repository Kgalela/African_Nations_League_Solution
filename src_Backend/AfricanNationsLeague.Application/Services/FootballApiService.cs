using System.Text.Json;

public class FootballApiService
{
    private readonly HttpClient _http;

    public FootballApiService(HttpClient http)
    {
        _http = http;
        _http.DefaultRequestHeaders.Add("x-apisports-key", "YOUR_API_KEY_HERE");
    }

    public async Task<List<string>> GetPlayersByCountryAsync(string countryName)
    {
        string url = $"https://v3.football.api-sports.io/players?search={countryName}&season=2024";

        var response = await _http.GetAsync(url);
        if (!response.IsSuccessStatusCode) return new();

        var json = await response.Content.ReadAsStringAsync();

        using var doc = JsonDocument.Parse(json);
        var root = doc.RootElement;

        var result = new List<string>();

        foreach (var item in root.GetProperty("response").EnumerateArray())
        {
            var playerName = item
                .GetProperty("player")
                .GetProperty("name")
                .GetString();

            if (!string.IsNullOrEmpty(playerName))
                result.Add(playerName);
        }

        return result.Distinct().ToList();
    }
}
