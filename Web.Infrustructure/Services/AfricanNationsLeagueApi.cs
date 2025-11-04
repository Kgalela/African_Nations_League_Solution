using System.Text;
using System.Text.Json;
using Web.Infrustructure.Models;

namespace Web.Infrustructure.Services
{
    public class AfricanNationsLeagueApi : IAfricanNationsLeagueApi
    {
        private readonly HttpClient _httpClient;
        private readonly JsonSerializerOptions _jsonOptions =
    new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

        public AfricanNationsLeagueApi(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }


        public async Task RegisterUser(UserDto dto)
        {
            var jsonContent = JsonSerializer.Serialize(dto,
                new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });

            var httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("/api/User/register", httpContent);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception($"API call failed: {response.StatusCode}, Details: {error}");
            }

        }

        public async Task<UserDto?> LoginUser(LoginDto login)
        {
            var jsonContent = JsonSerializer.Serialize(login,
                new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
            var httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("/api/User/login", httpContent);
            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception($"API call failed: {response.StatusCode}, Details: {error}");
            }
            var jsonResponse = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            return JsonSerializer.Deserialize<UserDto>(jsonResponse, options);
        }


        public async Task<List<Country>> GetCountriesFlags()

        {
            var response = await _httpClient.GetAsync("/api/Countries");
            response.EnsureSuccessStatusCode();
            var jsonResponse = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            var result = JsonSerializer.Deserialize<List<Country>>(jsonResponse, options);
            return result ?? new List<Country>();


        }

        public async Task<TeamDto> RegisterTeam(CreateTeamDto team)
        {
            var jsonContent = JsonSerializer.Serialize(team,
                new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });

            var httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("/api/Teams/register", httpContent);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception($"API call failed: {response.StatusCode}, Details: {error}");
            }

            var jsonResponse = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            return JsonSerializer.Deserialize<TeamDto>(jsonResponse, options)
                   ?? throw new Exception("Failed to deserialize TeamDto from API response.");
        }


        public async Task<UserDto?> GetUserByEmail(string email)
        {
            var response = await _httpClient.GetAsync($"/api/User/by-email?email={email}");
            response.EnsureSuccessStatusCode();
            var jsonResponse = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            var result = JsonSerializer.Deserialize<UserDto>(jsonResponse, options);
            return result;
        }


        public async Task<TournamentBracketDto?> GetTournamentAsync()
        {
            var response = await _httpClient.GetAsync("/api/tournament");
            if (!response.IsSuccessStatusCode)
                throw new Exception($"Failed to fetch tournament: {response.StatusCode}");

            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<TournamentBracketDto?>(json, _jsonOptions);
        }

        public async Task<TournamentBracketDto?> StartTournamentAsync()
        {
            var response = await _httpClient.PostAsync("/api/tournament/start", null);
            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception($"Start tournament failed: {response.StatusCode}, Details: {error}");
            }

            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<TournamentBracketDto?>(json, _jsonOptions);
        }

        public async Task<TournamentBracketDto?> SimulateStageAsync()
        {
            var response = await _httpClient.PostAsync("/api/tournament/simulate-stage", null);
            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception($"Simulate stage failed: {response.StatusCode}, Details: {error}");
            }

            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<TournamentBracketDto?>(json, _jsonOptions);
        }


        public async Task<List<TeamDto>> GetTeams()
        {
            var response = await _httpClient.GetAsync("/api/Teams");
            response.EnsureSuccessStatusCode();
            var jsonResponse = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            var result = JsonSerializer.Deserialize<List<TeamDto>>(jsonResponse, options);
            return result ?? new List<TeamDto>();
        }


        public async Task<MatchDto> GetMatchByID(string id)
        {
            var response = await _httpClient.GetAsync($"/api/Match/{id}");
            response.EnsureSuccessStatusCode();
            var jsonResponse = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            var result = JsonSerializer.Deserialize<MatchDto>(jsonResponse, options);
            return result ?? throw new Exception("Match not found");
        }

        public async Task<MatchDto> MathchSimulateMatch(string homeTeamId, string awayTeamId, string stage)
        {
            var response = await _httpClient.PostAsync($"/api/Match/simulate?homeTeamId={homeTeamId}&awayTeamId={awayTeamId}&stage={stage}", null);
            response.EnsureSuccessStatusCode();
            var jsonResponse = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            var result = JsonSerializer.Deserialize<MatchDto>(jsonResponse, options);
            return result ?? throw new Exception("Failed to simulate match");
        }


        public async Task<List<MatchDto>> GetAllMatches()
        {

            var response = await _httpClient.GetAsync("/api/Match/GetAllMatches");
            response.EnsureSuccessStatusCode();
            var jsonResponse = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            var result = JsonSerializer.Deserialize<List<MatchDto>>(jsonResponse, options);
            return result ?? new List<MatchDto>();
        }

        public async Task<TournamentBracketDto> SimulateStageSemiFinalsAsync(string homeId, string awayId)
        {
            var response = await _httpClient.PostAsync($"/api/Tournament/semifinals/simulate?homeId={homeId}&awayId={awayId}", null);
            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception($"Simulate semi-finals failed: {response.StatusCode}, Details: {error}");
            }
            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<TournamentBracketDto>(json, _jsonOptions)
                   ?? throw new Exception("Failed to deserialize TournamentBracketDto from API response.");
        }



    }
}
