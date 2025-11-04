using Microsoft.AspNetCore.Components;
using MudBlazor;
using Web.Infrustructure.Models;
using Web.Infrustructure.Services;

namespace AfricanNationsLeague_Web.Components.Pages
{
    public partial class TeamRegisteration
    {
        [Inject]
        public NavigationManager Navigation { get; set; }
        [Inject]
        public IAfricanNationsLeagueApi africanNationsLeagueApi { get; set; }

        Snackbar snackbar;

        public TeamDto team = new TeamDto();


        public List<Country> countries = new List<Country>();

        public string SelectedCountry { get; set; }

        [Parameter]
        public string email { get; set; }


        protected override async Task OnInitializedAsync()
        {
            if (africanNationsLeagueApi != null)
            {
                countries = await LoadCountryNames();


            }
        }

        public async Task<List<Country>> LoadCountryNames()
        {

            var results = await africanNationsLeagueApi.GetCountriesFlags();

            return results;

        }




        public async Task AutoGenerateSquadAsync()
        {
            if (string.IsNullOrEmpty(SelectedCountry))
            {
                Snackbar.Add("Please select a country.", Severity.Error);
                return;
            }

            var selectedCountryObj = countries.FirstOrDefault(c => c.Code == SelectedCountry);
            if (selectedCountryObj == null)
            {
                Snackbar.Add("Selected country not found.", Severity.Error);
                return;
            }

            var accountuser = await africanNationsLeagueApi.GetUserByEmail(email);
            if (accountuser == null)
            {
                Snackbar.Add("User not found for the provided email.", Severity.Error);
                return;
            }

            var teamRequest = new CreateTeamDto
            {
                Country = new Country
                {
                    Code = selectedCountryObj.Code,
                    Name = selectedCountryObj.Name,
                    FlagUrl = selectedCountryObj.FlagUrl
                },
                ManagerName = managerName,
                email = email,
            };

            // Call the API to generate the team and squad
            var createdTeam = await africanNationsLeagueApi.RegisterTeam(teamRequest);

            // Display the generated players and team info
            team = createdTeam;
            players = createdTeam.Players
                .Select(p => new PlayerModel
                {
                    Name = p.Name,
                    NaturalPosition = p.NaturalPosition,
                    GKRating = p.Ratings.ContainsKey("GK") ? p.Ratings["GK"] : 0,
                    DFRating = p.Ratings.ContainsKey("DF") ? p.Ratings["DF"] : 0,
                    MDRating = p.Ratings.ContainsKey("MD") ? p.Ratings["MD"] : 0,
                    ATRating = p.Ratings.ContainsKey("AT") ? p.Ratings["AT"] : 0,
                    IsCaptain = p.IsCaptain
                })
                .ToList();

            StateHasChanged();
        }

        private int CalculateTeamRating()
        {
            if (team == null || team.Players == null || !team.Players.Any())
                return 0;
            return (int)team.AverageRating;
        }

        private async Task SubmitRegistration()
        {
            // Save the team using the API
            var createdTeam = await africanNationsLeagueApi.RegisterTeam(new CreateTeamDto
            {
                Country = team.Country,
                ManagerName = team.ManagerName,
                email = team.email
            });



            team = createdTeam;
            players = createdTeam.Players
                .Select(p => new PlayerModel
                {
                    Name = p.Name,
                    NaturalPosition = p.NaturalPosition,
                    GKRating = p.Ratings.ContainsKey("GK") ? p.Ratings["GK"] : 0,
                    DFRating = p.Ratings.ContainsKey("DF") ? p.Ratings["DF"] : 0,
                    MDRating = p.Ratings.ContainsKey("MD") ? p.Ratings["MD"] : 0,
                    ATRating = p.Ratings.ContainsKey("AT") ? p.Ratings["AT"] : 0,
                    IsCaptain = p.IsCaptain
                })
                .ToList();
            Snackbar.Add("Team registered successfully!", Severity.Success);
            StateHasChanged();

        }


        private string GetPositionName(string position)
        {
            return position switch
            {
                "GK" => "Goalkeepers",
                "DF" => "Defenders",
                "MD" => "Midfielders",
                "AT" => "Attackers",
                _ => position
            };
        }





        private MudStepper stepper;
        //private string selectedCountry = "";
        private string managerName = "";
        //private string representativeName = email;
        public List<PlayerModel> players = new();




        public class PlayerModel
        {
            public string Name { get; set; }
            public string NaturalPosition { get; set; }
            public int GKRating { get; set; }
            public int DFRating { get; set; }
            public int MDRating { get; set; }
            public int ATRating { get; set; }
            public bool IsCaptain { get; set; }
        }
    }
}
