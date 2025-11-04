namespace Web.Infrustructure.Models
{
    public class TeamDto
    {
        public string? Id { get; set; }
        public Country Country { get; set; }
        public string ManagerName { get; set; } = string.Empty;
        public string email { get; set; } = string.Empty;
        public double AverageRating { get; set; }
        public List<Player> Players { get; set; } = new();
        public DateTime CreatedAt { get; set; }
    }
}
