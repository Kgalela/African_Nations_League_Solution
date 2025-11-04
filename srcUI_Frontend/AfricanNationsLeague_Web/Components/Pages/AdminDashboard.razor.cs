using Microsoft.AspNetCore.Components;
using Web.Infrustructure.Models;
using Web.Infrustructure.Services;

namespace AfricanNationsLeague_Web.Components.Pages
{
    public partial class AdminDashboard
    {
        [Inject]
        private NavigationManager? Navigation { get; set; }

        [Inject]
        public IAfricanNationsLeagueApi? africanNationsLeagueApi { get; set; }

        public List<TeamDto> TeamsRegistred = new List<TeamDto>();

        public List<Country> countries = new List<Country>();

        public List<MatchDto> MatchesList = new List<MatchDto>();



        public MatchDto? GetMatch { get; set; } = null;

        public MatchDto? PlayMatch { get; set; } = null;

        TournamentBracketDto? tournament;
        List<MatchDto> quarterFinals = new();
        List<MatchDto> semiFinals = new();
        MatchDto? finalMatch;
        MatchDto Match;

        public string? FindMatchId(string homeTeamId, string awayTeamId)
        {
            // Compare against MatchesList (from GetAllMatches)
            var match = MatchesList.FirstOrDefault(m =>
                m.HomeTeamId == homeTeamId && m.AwayTeamId == awayTeamId);

            // If not found, try reverse (in case teams are swapped)
            if (match == null)
            {
                match = MatchesList.FirstOrDefault(m =>
                    m.HomeTeamId == awayTeamId && m.AwayTeamId == homeTeamId);
            }

            return match?.Id;
        }


        private async Task NavigateToMatchDetails(string MatchId)
        {

            Console.WriteLine($"Navigating to match with ID: {MatchId}"); // Should print a valid ID
            Navigation.NavigateTo($"/match/{MatchId}");
        }

        private bool CanStartTournament => TeamsRegistred.Count >= 8;

        protected override async Task OnInitializedAsync()
        {
            if (africanNationsLeagueApi != null)
            {
                countries = await LoadCountryNames();
                TeamsRegistred = await LoadRegisteredTeamsAsync();

                tournament = await LoadTournamentBracketAsync();
                MatchesList = await LoadAllMatchesAsync();

            }
        }

        public async Task<List<Country>> LoadCountryNames()
        {
            var results = await africanNationsLeagueApi?.GetCountriesFlags() ?? new List<Country>();
            return results ?? new List<Country>();
        }

        public async Task<List<TeamDto>> LoadRegisteredTeamsAsync()
        {
            var results = await africanNationsLeagueApi?.GetTeams() ?? new List<TeamDto>();
            return results ?? new List<TeamDto>();
        }

        public async Task<TournamentBracketDto> LoadTournamentBracketAsync()
        {
            var results = await africanNationsLeagueApi?.GetTournamentAsync();
            return results ?? new TournamentBracketDto { Matches = new List<MatchDto>() };
        }

        private async Task LoadTournament()
        {
            tournament = await africanNationsLeagueApi?.GetTournamentAsync();

            if (tournament != null)
            {
                quarterFinals = tournament.Matches
                    .Where(m => m.Stage == "Quarterfinal")
                    .ToList();

                semiFinals = tournament.Matches
                    .Where(m => m.Stage == "Semifinal")
                    .ToList();

                finalMatch = tournament.Matches
                    .FirstOrDefault(m => m.Stage == "Final");
            }

            StateHasChanged();
        }

        private async Task StartTournament()
        {
            tournament = await africanNationsLeagueApi?.StartTournamentAsync();
            await LoadTournament();
        }

        private async Task SimulateStage()
        {
            tournament = await africanNationsLeagueApi?.SimulateStageAsync();
            await LoadTournament();
        }


        private async Task<MatchDto> PlayMatchAction(string homeTeamId, string awayTeamId, string stage)
        {
            var response = await africanNationsLeagueApi.MathchSimulateMatch(homeTeamId, awayTeamId, stage);

            return response;
        }

        public async Task<List<MatchDto>> LoadAllMatchesAsync()
        {
            var results = await africanNationsLeagueApi?.GetAllMatches() ?? new List<MatchDto>();
            return results.ToList();
        }

        TournamentBracketDto? semiFinal;
        public async Task SemiFinalSimlate(string homeId, string awayId)
        {
            semiFinal = await africanNationsLeagueApi.SimulateStageSemiFinalsAsync(homeId, awayId);
            await LoadTournament();
            StateHasChanged();
        }


    }
}
