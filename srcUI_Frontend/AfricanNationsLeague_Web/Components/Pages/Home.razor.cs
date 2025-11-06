using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace AfricanNationsLeague_Web.Components.Pages
{
    public partial class Home
    {
        [Inject]
        public NavigationManager navigationManager { get; set; }

        private TournamentBracket tournamentBracketRef;

        private async Task HandleTeamRegistered()
        {
            if (tournamentBracketRef != null)
                await tournamentBracketRef.ReloadAsync();
        }

        public void NavigateToRegisterTeam()
        {
            navigationManager.NavigateTo("/login");
        }

        public void NavigateToRegisterUser()
        {
            navigationManager.NavigateTo("/register");
        }

        [Parameter]
        public string? ScrollTo { get; set; }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender && !string.IsNullOrEmpty(ScrollTo))
            {
                await JS.InvokeVoidAsync("scrollToElement", ScrollTo);
            }
        }
    }
}
