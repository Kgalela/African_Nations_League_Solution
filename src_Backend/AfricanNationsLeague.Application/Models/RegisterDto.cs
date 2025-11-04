using AfricanNationsLeague.Domain.Entities;
using AfricanNationsLeague.Domain.Enums;

namespace AfricanNationsLeague.Application.Models
{
    public class RegisterDto
    {
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public Country Country { get; set; }
        public Role Role { get; set; } = Role.Representative;
    }
}
