using AfricanNationsLeague.Domain.Common;

namespace AfricanNationsLeague.Domain.Entities
{
    public class Player : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string NaturalPosition { get; set; } = string.Empty; // store enum as string too
        public Dictionary<string, int> Ratings { get; set; } = new();
        public bool IsCaptain { get; set; } = false;
    }
}
