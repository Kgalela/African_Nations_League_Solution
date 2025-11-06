namespace Web.Infrustructure.Models
{
    public record SendEmailRequestDto(string To, string Subject, string Body)
    {
    }
}
