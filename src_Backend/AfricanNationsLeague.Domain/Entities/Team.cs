using AfricanNationsLeague.Domain.Common;

namespace AfricanNationsLeague.Domain.Entities
{
    public class Team : BaseEntity
    {

        public Country Country { get; set; }
        public string ManagerName { get; set; } = string.Empty;
        public string email { get; set; } = string.Empty; // link to representative
        public List<Player> Players { get; set; } = new();
        public double AverageRating { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    }
}
