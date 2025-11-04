using AfricanNationsLeague.Application.Services;
using AfricanNationsLeague.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace AfricanNationsLeague.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MatchController : ControllerBase
    {
        private readonly MatchService _service;

        public MatchController(MatchService service)
        {
            _service = service;
        }

        [HttpPost("simulate")]
        public async Task<IActionResult> Simulate([FromQuery] string homeTeamId, [FromQuery] string awayTeamId, string stage)
        {
            try
            {
                var match = await _service.SimulateMatchAsync(homeTeamId, awayTeamId, stage);
                return Ok(match);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var match = await _service.GetByIdAsync(id);
            if (match == null) return NotFound();
            return Ok(match);
        }

        [HttpGet("GetAllMatches")]
        public async Task<IActionResult> GetAll()
        {
            var matches = await _service.GetAllAsync();
            return Ok(matches);
        }

        [HttpPost("simulatingbyID/{matchId}")]
        public async Task<ActionResult<Match>> SimulateMatch(string matchId)
        {
            var result = await _service.SimulateMatchByIdAsync(matchId);
            return Ok(result);
        }



        //✅ Play match with commentary
        [HttpPost("play/{Id}")]
        public async Task<ActionResult<Match>> PlayMatch(string Id)
        {

            var result = await _service.PlayMatchByIdAsync(Id);
            return Ok(result);
        }


        //[HttpPost("semifinals/{matchId}/simulate")]
        //public async Task<IActionResult> SimulateSemiFinalMatch(string matchId)
        //{
        //    var match = await _service.PlayMatchByIdAsync(matchId);
        //    return Ok(match);
        //}



    }
}
