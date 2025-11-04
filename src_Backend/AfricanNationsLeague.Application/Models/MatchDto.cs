using System.Text.Json.Serialization;

namespace AfricanNationsLeague.Application.Models
{
    public class MatchDto
    {
        [JsonPropertyName("Id")]
        public string Id { get; set; }



        public string HomeTeamId { get; set; }
        public string AwayTeamId { get; set; }

        public CountriesDto HomeCountry { get; set; }
        public CountriesDto AwayCountry { get; set; }

        public int HomeScore { get; set; }
        public int AwayScore { get; set; }

        public List<GoalDto> HomeGoals { get; set; } = new();
        public List<GoalDto> AwayGoals { get; set; } = new();

        public List<CommentaryEventDto> Commentary { get; set; } = new();

        public string WinnerCountryCode { get; set; }
        public string Stage { get; set; }
        public bool IsPlayed { get; set; }
        public DateTime? PlayedAt { get; set; }
        public string Status { get; internal set; }
    }
}
