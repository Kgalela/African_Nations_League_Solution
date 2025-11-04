using System.Text.Json.Serialization;

namespace Web.Infrustructure.Models
{
    public class MatchDto
    {
        [JsonPropertyName("Id")]
        public string Id { get; set; } = string.Empty;



        public string HomeTeamId { get; set; }
        public string AwayTeamId { get; set; }

        public Country HomeCountry { get; set; }
        public Country AwayCountry { get; set; }

        public int HomeScore { get; set; }
        public int AwayScore { get; set; }

        public List<GoalDto> HomeGoals { get; set; } = new();
        public List<GoalDto> AwayGoals { get; set; } = new();

        public List<CommentaryEventDto> Commentary { get; set; } = new();

        public string WinnerCountryCode { get; set; }
        public string Stage { get; set; }
        public bool IsPlayed { get; set; }
        public DateTime? PlayedAt { get; set; }

    }

}
