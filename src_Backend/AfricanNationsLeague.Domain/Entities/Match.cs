using AfricanNationsLeague.Domain.Common;

namespace AfricanNationsLeague.Domain.Entities
{

    public class Match : BaseEntity
    {
        public string HomeTeamId { get; set; } = string.Empty;
        public string AwayTeamId { get; set; } = string.Empty;

        public Country HomeCountry { get; set; } = new();
        public Country AwayCountry { get; set; } = new();

        public int HomeScore { get; set; } = -1;
        public int AwayScore { get; set; } = -1;

        public List<Goal> HomeGoals { get; set; } = new();
        public List<Goal> AwayGoals { get; set; } = new();
        public List<CommentaryEvent> Commentary { get; set; } = new();

        public string WinnerCountryCode { get; set; } = string.Empty;
        public string Stage { get; set; } = string.Empty;

        public bool IsPlayed { get; set; } = false;
        public DateTime? PlayedAt { get; set; }


    }



}
