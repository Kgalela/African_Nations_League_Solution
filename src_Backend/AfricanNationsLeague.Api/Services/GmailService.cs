using AfricanNationsLeague.Api.Abstracts;
using AfricanNationsLeague.Api.Contracts;
using AfricanNationsLeague.Api.Options;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;

namespace AfricanNationsLeague.Api.Services
{
    public class GmailService : IMailService
    {
        private readonly GmailOptions _options;

        public GmailService(IOptions<GmailOptions> options)
        {
            _options = options.Value;
        }


        public async Task SendEmailAsynu(SendEmailRequest request)
        {
            using var client = new SmtpClient(_options.Host, _options.Port)
            {
                Credentials = new NetworkCredential(_options.Email, _options.Password),
                EnableSsl = true
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress(_options.Email),
                Subject = request.Subject,
                Body = request.Body,
                IsBodyHtml = true
            };
            mailMessage.To.Add(request.To);
            await client.SendMailAsync(mailMessage);

        }
    }
}
