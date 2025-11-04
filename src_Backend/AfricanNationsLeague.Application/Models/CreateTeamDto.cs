using AfricanNationsLeague.Domain.Entities;

namespace AfricanNationsLeague.Application.Models
{
    public class CreateTeamDto
    {
        public Country Country { get; set; }
        public string ManagerName { get; set; } = string.Empty;
        public string email { get; set; } = string.Empty;
    }
}
