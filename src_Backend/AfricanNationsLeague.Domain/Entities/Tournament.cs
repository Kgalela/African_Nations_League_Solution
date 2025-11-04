using AfricanNationsLeague.Domain.Common;

namespace AfricanNationsLeague.Domain.Entities
{
    public class Tournament : BaseEntity
    {
        public string Name { get; set; } = "African Nations League 2026";
        public List<Match> Matches { get; set; } = new();
        public string CurrentStage { get; set; } = "Quarterfinal";
        public bool IsCompleted { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }



    //public class Tournament : BaseEntity
    //{

    //    public string Name { get; set; } = "African Nations League 2025";
    //    public List<string> TeamIds { get; set; } = new();
    //    public List<Match> Matches { get; set; } = new();
    //    public string? WinnerTeamId { get; set; }
    //    public string CurrentStage { get; set; } = "Quarterfinal";
    //    public bool IsCompleted { get; set; } = false;
    //    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    //}
}
