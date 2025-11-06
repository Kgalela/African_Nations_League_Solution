using Microsoft.AspNetCore.Components;
using MudBlazor;
using Web.Infrustructure.Models;
using Web.Infrustructure.Services;

namespace AfricanNationsLeague_Web.Components.Pages
{
    public partial class Results
    {
        [Inject]
        private NavigationManager? Navigation { get; set; }

        [Inject]
        public IAfricanNationsLeagueApi? africanNationsLeagueApi { get; set; }



        public List<Country> countries = new List<Country>();

        public List<MatchDto> MatchesList = new List<MatchDto>();

        public MatchDto? GetMatch { get; set; } = null;

        public MatchDto? PlayMatch { get; set; } = null;

        //public bool selectedMatch { get; set; } = false;

        //public bool showModal { get; set; } = false;

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

        private MudMessageBox _mudMessageBox;
        private MatchDto? selectedMatch;

        private async Task ShowScoreline(MatchDto match)
        {
            selectedMatch = match;
            await _mudMessageBox.ShowAsync();
        }


        // Results.razor.cs




        protected override async Task OnInitializedAsync()
        {
            if (africanNationsLeagueApi != null)
            {
                countries = await LoadCountryNames();


                //tournament = await LoadTournamentBracketAsync();
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

        //private async Task StartTournament()
        //{
        //    tournament = await africanNationsLeagueApi?.StartTournamentAsync();
        //    await LoadTournament();
        //}

        //private async Task SimulateStage()
        //{
        //    tournament = await africanNationsLeagueApi?.SimulateStageAsync();
        //    await LoadTournament();
        //}

        //private async Task<MatchDto> PlayMatchAction(string homeTeamId, string awayTeamId, string stage)
        //{
        //    var response = await africanNationsLeagueApi.MathchSimulateMatch(homeTeamId, awayTeamId, stage);

        //    return response;
        //}




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
