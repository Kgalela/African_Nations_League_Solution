using Microsoft.AspNetCore.Components;
using Web.Infrustructure.Models;
using Web.Infrustructure.Services;

namespace AfricanNationsLeague_Web.Components.Pages
{
    public partial class TournamentBracket
    {
        [Inject] public IAfricanNationsLeagueApi africanNationsLeagueApi { get; set; }

        TournamentBracketDto? tournament;
        List<MatchDto> quarterFinals = new();
        List<MatchDto> semiFinals = new();
        MatchDto? finalMatch;
        TournamentBracketDto? semiFinal;



        protected override async Task OnInitializedAsync()
        {
            await LoadTournament();
        }

        private async Task LoadTournament()
        {
            tournament = await africanNationsLeagueApi.GetTournamentAsync();

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

        // Add this method here
        public async Task ReloadAsync()
        {
            tournament = await africanNationsLeagueApi.GetTournamentAsync();
            StateHasChanged();
        }
    }
}
