using AfricanNationsLeague.Domain.Common;
using AfricanNationsLeague.Domain.Enums;

namespace AfricanNationsLeague.Domain.Entities
{
    public class User : BaseEntity
    {

        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public Role Role { get; set; }
        public Country Country { get; set; } // For federation representative

        public DateTime CreatedAt { get; set; }

    }
}
