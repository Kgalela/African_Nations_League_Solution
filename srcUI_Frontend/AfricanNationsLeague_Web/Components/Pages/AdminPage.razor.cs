//using Microsoft.AspNetCore.Components;
//using Web.Infrustructure.Models;
//using Web.Infrustructure.Services;

//namespace AfricanNationsLeague_Web.Components.Pages
//{
//    public partial class AdminPage
//    {

//        [Inject]
//        public NavigationManager? NavigationManager { get; set; }

//        [Inject]
//        public IAfricanNationsLeagueApi? africanNationsLeagueApi { get; set; }

//        public List<TeamDto> TeamsRegistred = new List<TeamDto>();

//        public List<Country> countries = new List<Country>();

//        TournamentBracketDto? tournament;
//        List<MatchDto> quarterFinals = new();
//        List<MatchDto> semiFinals = new();
//        MatchDto? finalMatch;



//        protected override async Task OnInitializedAsync()
//        {
//            if (africanNationsLeagueApi != null)
//            {
//                countries = await LoadCountryNames();
//                TeamsRegistred = await LoadRegisteredTeamsAsync();
//                await LoadTournament();

//            }
//        }

//        public async Task<List<Country>> LoadCountryNames()
//        {

//            var results = await africanNationsLeagueApi.GetCountriesFlags();

//            return results;

//        }


//        public async Task<List<TeamDto>> LoadRegisteredTeamsAsync()
//        {

//            var results = await africanNationsLeagueApi.GetTeams();

//            return results;

//        }

//        private async Task LoadTournament()
//        {
//            tournament = await africanNationsLeagueApi.GetTournamentAsync();

//            if (tournament != null)
//            {
//                quarterFinals = tournament.Matches
//                    .Where(m => m.Stage == "Quarterfinal")
//                    .ToList();

//                semiFinals = tournament.Matches
//                    .Where(m => m.Stage == "Semifinal")
//                    .ToList();

//                finalMatch = tournament.Matches
//                    .FirstOrDefault(m => m.Stage == "Final");
//            }

//            StateHasChanged();
//        }

//        private async Task StartTournament()
//        {
//            tournament = await africanNationsLeagueApi.StartTournamentAsync();
//            await LoadTournament();
//        }

//        private async Task SimulateStage()
//        {
//            tournament = await africanNationsLeagueApi.SimulateStageAsync();
//            await LoadTournament();
//        }




























//    }
//}
