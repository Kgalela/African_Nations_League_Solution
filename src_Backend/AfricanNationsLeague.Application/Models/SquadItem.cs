namespace AfricanNationsLeague.Application.Models
{
    public class SquadItem
    {
        public TeamObj team { get; set; }
        public List<PlayerApiItem> players { get; set; } = new();
    }
}
