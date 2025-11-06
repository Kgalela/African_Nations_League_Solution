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

        public TeamDto team = new TeamDto();

        public MatchDto? GetMatch { get; set; } = null;

        public MatchDto? PlayMatch { get; set; } = null;

        TournamentBracketDto? tournament;
        List<MatchDto> quarterFinals = new();
        List<MatchDto> semiFinals = new();
        MatchDto? finalMatch;
        MatchDto Match;

        [Parameter]
        public string email { get; set; }




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

            Navigation?.NavigateTo(Navigation.Uri, forceLoad: true); // Reloads the page

        }

        private async Task StartTournament()
        {
            tournament = await africanNationsLeagueApi?.StartTournamentAsync();
            await LoadTournament();
            StateHasChanged();
        }

        private async Task ResetTournament()
        {
            tournament = await africanNationsLeagueApi?.RestartTournamentAsync();
            TeamsRegistred = await LoadRegisteredTeamsAsync();
            await LoadTournament();
            Navigation?.NavigateTo(Navigation.Uri, forceLoad: true);

        }

        //private async Task SimulateStage()
        //{
        //    tournament = await africanNationsLeagueApi?.SimulateStageAsync();
        //    await LoadTournament();
        //    Navigation?.NavigateTo(Navigation.Uri, forceLoad: true); // Reloads the page
        //}

        private async Task SimulateStage()
        {
            tournament = await africanNationsLeagueApi?.SimulateStageAsync();
            StateHasChanged();
            // Send email to managers of teams that played in the current stage
            if (tournament != null && tournament.Matches != null)
            {
                var recentMatches = tournament.Matches
                    .Where(m => m.IsPlayed && m.PlayedAt.HasValue && m.PlayedAt.Value.Date == DateTime.Today)
                    .ToList();

                foreach (var match in recentMatches)
                {
                    var homeTeam = TeamsRegistred.FirstOrDefault(t => t.Id == match.HomeTeamId);
                    var awayTeam = TeamsRegistred.FirstOrDefault(t => t.Id == match.AwayTeamId);

                    foreach (var team in new[] { homeTeam, awayTeam })
                    {
                        // Use team.email for each manager, not the page parameter 'email'
                        if (team != null && !string.IsNullOrWhiteSpace(email))
                        {
                            var goalsHome = match.HomeGoals?.Select(g => $"{g.PlayerName}").ToList() ?? new List<string>();
                            var goalsAway = match.AwayGoals?.Select(g => $"{g.PlayerName}").ToList() ?? new List<string>();

                            var body = $@"
                                Hi {team.ManagerName},

                                Here are the match results for your team:

                                🏆 {match.Stage}: {match.HomeCountry?.Name} vs {match.AwayCountry?.Name}
                                Final Score: {match.HomeCountry?.Name} {match.HomeScore} - {match.AwayScore} {match.AwayCountry?.Name}

                                Goals:
                                • {match.HomeCountry?.Name}: {(goalsHome.Count > 0 ? string.Join(", ", goalsHome) : "None")}
                                • {match.AwayCountry?.Name}: {(goalsAway.Count > 0 ? string.Join(", ", goalsAway) : "None")}

                                Winner: {(match.WinnerCountryCode == match.HomeCountry?.Code ? match.HomeCountry?.Name : match.AwayCountry?.Name)}
                                Played on: {match.PlayedAt?.ToString("dd MMM yyyy")}

                                Thank you for participating in the African Nations League.

                                Regards,
                                Tournament Admin
                                ";

                            var emailResult = new SendEmailRequestDto(
                                email,
                                $"Match Results: {match.HomeCountry?.Name} vs {match.AwayCountry?.Name}",
                                body
                            );

                            await africanNationsLeagueApi.SendEmail(emailResult);
                        }
                    }
                }
                await LoadTournament();
                Navigation?.NavigateTo(Navigation.Uri, forceLoad: true);
                StateHasChanged();
            }
        }


        //private async Task SimulateStage()
        //{
        //    tournament = await africanNationsLeagueApi?.SimulateStageAsync();


        //    // Send email to managers of teams that played in the current stage
        //    if (tournament != null && tournament.Matches != null)
        //    {
        //        // Get matches just played (IsPlayed == true, PlayedAt is recent)
        //        var recentMatches = tournament.Matches
        //            .Where(m => m.IsPlayed && m.PlayedAt.HasValue && m.PlayedAt.Value.Date == DateTime.Today)
        //            .ToList();

        //        foreach (var match in recentMatches)
        //        {
        //            // Find teams
        //            var homeTeam = TeamsRegistred.FirstOrDefault(t => t.Id == match.HomeTeamId);
        //            var awayTeam = TeamsRegistred.FirstOrDefault(t => t.Id == match.AwayTeamId);

        //            // Compose email for both managers
        //            foreach (var team in new[] { homeTeam, awayTeam })
        //            {
        //                if (team != null && !string.IsNullOrWhiteSpace(email))
        //                {
        //                    var opponent = team == homeTeam ? awayTeam : homeTeam;
        //                    var goalsHome = match.HomeGoals?.Select(g => $"{g.PlayerName}").ToList() ?? new List<string>();
        //                    var goalsAway = match.AwayGoals?.Select(g => $"{g.PlayerName}").ToList() ?? new List<string>();

        //                    var body = $@"
        //                        Hi {team.ManagerName},

        //                        Here are the match results for your team:

        //                        🏆 {match.Stage}: {match.HomeCountry?.Name} vs {match.AwayCountry?.Name}
        //                        Final Score: {match.HomeCountry?.Name} {match.HomeScore} - {match.AwayScore} {match.AwayCountry?.Name}

        //                        Goals:
        //                        • {match.HomeCountry?.Name}: {(goalsHome.Count > 0 ? string.Join(", ", goalsHome) : "None")}
        //                        • {match.AwayCountry?.Name}: {(goalsAway.Count > 0 ? string.Join(", ", goalsAway) : "None")}

        //                        Winner: {(match.WinnerCountryCode == match.HomeCountry?.Code ? match.HomeCountry?.Name : match.AwayCountry?.Name)}
        //                        Played on: {match.PlayedAt?.ToString("dd MMM yyyy")}

        //                        Thank you for participating in the African Nations League.

        //                        Regards,
        //                        Tournament Admin
        //                        ";

        //                    var emailResult = new SendEmailRequestDto(
        //                        email,
        //                        $"Match Results: {match.HomeCountry?.Name} vs {match.AwayCountry?.Name}",
        //                        body

        //                        );

        //                    await africanNationsLeagueApi.SendEmail(emailResult);
        //                    await LoadTournament();
        //                    Navigation?.NavigateTo(Navigation.Uri, forceLoad: true); // Reloads the page
        //                }
        //            }
        //        }
        //    }
        //}



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
            if (semiFinal != null)
            {
                Console.WriteLine($"Stage: {semiFinal.CurrentStage}");
                Console.WriteLine($"Matches count: {semiFinal.Matches?.Count ?? 0}");
                foreach (var match in semiFinal.Matches)
                {
                    Console.WriteLine($"Match: {match.HomeCountry?.Name} vs {match.AwayCountry?.Name}, Score: {match.HomeScore}-{match.AwayScore}, Played: {match.IsPlayed}");
                }
            }
            else
            {
                Console.WriteLine("semiFinal is null.");
            }

            Navigation?.NavigateTo(Navigation.Uri, forceLoad: true); // Reloads the page

        }

        //        public async Task SemiFinalSimlate(string homeId, string awayId)
        //        {
        //            semiFinal = await africanNationsLeagueApi?.SimulateStageSemiFinalsAsync(homeId, awayId);
        //            StateHasChanged();
        //            if (semiFinal == null || semiFinal.Matches == null)
        //            {
        //                // Handle error or show message to user
        //                Console.Error.WriteLine("SemiFinal or its Matches is null.");
        //                return;
        //            }

        //            var match = semiFinal.Matches.FirstOrDefault(m =>
        //                m.HomeTeamId == homeId && m.AwayTeamId == awayId);

        //            if (match == null)
        //            {
        //                // Handle error or show message to user
        //                Console.Error.WriteLine("Match not found for given team IDs.");
        //                return;
        //            }

        //            var matches = await africanNationsLeagueApi?.GetMatchByID(match.Id);
        //            if (matches == null)
        //            {
        //                // Handle error or show message to user
        //                Console.Error.WriteLine("Match details not found for match ID.");
        //                return;
        //            }

        //            var goalsHome = matches.HomeGoals?.Select(g => $"{g.PlayerName}").ToList() ?? new List<string>();
        //            var goalsAway = matches.AwayGoals?.Select(g => $"{g.PlayerName}").ToList() ?? new List<string>();
        //            var body = $@"
        //Hi {team.ManagerName},

        //Here are the match results for your team:

        //🏆 {matches.Stage}: {matches.HomeCountry?.Name} vs {matches.AwayCountry?.Name}
        //Final Score: {matches.HomeCountry?.Name} {matches.HomeScore} - {matches.AwayScore} {matches.AwayCountry?.Name}

        //Goals:
        //• {matches.HomeCountry?.Name}: {(goalsHome.Count > 0 ? string.Join(", ", goalsHome) : "None")}
        //• {matches.AwayCountry?.Name}: {(goalsAway.Count > 0 ? string.Join(", ", goalsAway) : "None")}

        //Winner: {(match.WinnerCountryCode == match.HomeCountry?.Code ? match.HomeCountry?.Name : match.AwayCountry?.Name)}
        //Played on: {match.PlayedAt?.ToString("dd MMM yyyy")}

        //Thank you for participating in the African Nations League.

        //Regards,
        //Tournament Admin
        //";

        //            var emailRequest = new SendEmailRequestDto(
        //                email,
        //                $"Match Results: {matches.HomeCountry?.Name} vs {matches.AwayCountry?.Name}",
        //                body
        //            );

        //            await africanNationsLeagueApi.SendEmail(emailRequest);
        //            await LoadTournament();
        //            Navigation?.NavigateTo(Navigation.Uri, forceLoad: true);
        //            StateHasChanged();
        //        }

        //    }
    }
}


