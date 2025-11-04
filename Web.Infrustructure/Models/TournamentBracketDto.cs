namespace Web.Infrustructure.Models
{
    public class TournamentBracketDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string CurrentStage { get; set; }
        public bool IsCompleted { get; set; }
        public List<MatchDto> Matches { get; set; } = new();
    }
}
