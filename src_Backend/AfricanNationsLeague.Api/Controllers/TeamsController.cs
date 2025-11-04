using AfricanNationsLeague.Application.Models;
using AfricanNationsLeague.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace AfricanNationsLeague.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeamsController : ControllerBase
    {
        private readonly TeamService _service;

        public TeamsController(TeamService service)
        {
            _service = service;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Create([FromBody] CreateTeamDto dto)
        {
            try
            {
                var createdTeam = await _service.CreateTeamAsync(dto);
                return Ok(createdTeam);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var teams = await _service.GetAllAsync();
            return Ok(teams);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var team = await _service.GetByIdAsync(id);
            if (team == null)
                return NotFound();
            return Ok(team);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            await _service.DeleteAsync(id);
            return NoContent();
        }
    }
}
