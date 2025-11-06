using AfricanNationsLeague.Api.Contracts;

namespace AfricanNationsLeague.Api.Abstracts
{
    public interface IMailService
    {
        Task SendEmailAsynu(SendEmailRequest sendEmailRequest);
    }
}
