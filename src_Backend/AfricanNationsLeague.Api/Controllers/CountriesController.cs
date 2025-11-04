using AfricanNationsLeague.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace AfricanNationsLeague.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountriesController : ControllerBase
    {
        private readonly CountryService _service;

        public CountriesController(CountryService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _service.GetAllAsync();
            return Ok(result);
        }


    }
}
