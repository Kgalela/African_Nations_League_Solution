namespace AfricanNationsLeague.Application.Models
{
    public class ApiResponse<T>
    {
        public List<T> response { get; set; } = new();
    }
}
