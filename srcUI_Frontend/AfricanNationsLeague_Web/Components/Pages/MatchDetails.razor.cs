using Microsoft.AspNetCore.Components;
using Web.Infrustructure.Models;
using Web.Infrustructure.Services;

namespace AfricanNationsLeague_Web.Components.Pages
{
    public partial class MatchDetails
    {
        //    [Inject]
        //    public IAfricanNationsLeagueApi africanNationsLeagueApi { get; set; }

        //    [Parameter]
        //    public string MatchId { get; set; }

        //    private MatchDto match;
        //    private bool isLoading = true;
        //    private bool notFound = false;

        //    protected override async Task OnParametersSetAsync()
        //    {
        //        await LoadMatchDetails();
        //    }

        //    private async Task LoadMatchDetails()
        //    {
        //        try
        //        {
        //            if (string.IsNullOrWhiteSpace(MatchId))
        //            {
        //                notFound = true;
        //                return;
        //            }

        //            var results = await africanNationsLeagueApi.GetMatchByID(MatchId);

        //            if (results == null)
        //            {
        //                notFound = true;
        //                return;
        //            }

        //            match = new MatchDto
        //            {
        //                Id = results.Id,
        //                HomeTeamId = results.HomeTeamId,
        //                AwayTeamId = results.AwayTeamId,
        //                HomeCountry = results.HomeCountry,
        //                AwayCountry = results.AwayCountry,
        //                HomeScore = results.HomeScore,
        //                AwayScore = results.AwayScore,
        //                IsPlayed = results.IsPlayed,
        //                Stage = results.Stage,
        //                PlayedAt = results.PlayedAt,
        //                AwayGoals = results.AwayGoals,
        //                HomeGoals = results.HomeGoals,
        //                Commentary = results.Commentary,
        //                WinnerCountryCode = results.WinnerCountryCode
        //            };
        //        }
        //        catch (Exception)
        //        {
        //            notFound = true;
        //        }
        //        finally
        //        {
        //            isLoading = false;
        //            StateHasChanged();
        //        }
        //    }
        //}
        [Inject]
        public IAfricanNationsLeagueApi africanNationsLeagueApi { get; set; }

        [Parameter]
        public string MatchId { get; set; }

        private MatchDto match;
        private bool isLoading = true;
        private bool notFound = false;

        protected override async Task OnParametersSetAsync()
        {
            if (string.IsNullOrEmpty(MatchId))
            {
                Console.WriteLine("MatchId is null or empty — check navigation URL.");
                notFound = true;
                isLoading = false;
                return;
            }

            Console.WriteLine($"Loading match with ID: {MatchId}");

            try
            {
                match = await LoadMatchDetails();
                if (match == null)
                {
                    notFound = true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading match: {ex.Message}");
                notFound = true;
            }
            finally
            {
                isLoading = false;
            }
        }

        private async Task<MatchDto> LoadMatchDetails()
        {
            var result = await africanNationsLeagueApi.GetMatchByID(MatchId);
            if (result == null)
            {
                Console.WriteLine($"No match found for ID: {MatchId}");
                return null;
            }

            return new MatchDto
            {
                Id = result.Id,
                HomeTeamId = result.HomeTeamId,
                AwayTeamId = result.AwayTeamId,
                HomeCountry = result.HomeCountry,
                AwayCountry = result.AwayCountry,
                HomeScore = result.HomeScore,
                AwayScore = result.AwayScore,
                IsPlayed = result.IsPlayed,
                Stage = result.Stage,
                PlayedAt = result.PlayedAt,
                AwayGoals = result.AwayGoals,
                HomeGoals = result.HomeGoals,
                Commentary = result.Commentary,
                WinnerCountryCode = result.WinnerCountryCode
            };
        }


    }
}
