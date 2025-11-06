using AfricanNationsLeague.Api.Abstracts;
using AfricanNationsLeague.Api.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace AfricanNationsLeague.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SendEmailController : ControllerBase
    {
        private readonly IMailService _mailService;

        public SendEmailController(IMailService mailService)
        {
            _mailService = mailService;
        }

        [HttpPost("email")]

        public async Task<IActionResult> SendEmailToUser(SendEmailRequest sendEmailRequest)
        {

            try
            {
                await _mailService.SendEmailAsynu(sendEmailRequest);
                return Ok("Email sent successfully.");
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }
    }
}
