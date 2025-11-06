using AfricanNationsLeague.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class TournamentController : ControllerBase
{
    private readonly TournamentService _service;

    public TournamentController(TournamentService service)
    {
        _service = service;
    }

    [HttpGet("semifinals")]
    public async Task<IActionResult> GetSemiFinalMatches()
    {
        var matches = await _service.GetSemiFinalMatchesAsync();
        return Ok(matches.Select(m => new { m.Id, m.HomeCountry, m.AwayCountry, m.IsPlayed }));
    }

    //[HttpGet]
    //public async Task<IActionResult> Get() =>
    //    Ok(await _service.GetActiveAsync());

    [HttpPost("start")]
    public async Task<IActionResult> StartTournament() =>
        Ok(await _service.StartTournamentAsync());

    [HttpPost("simulate-stage")]
    public async Task<IActionResult> SimulateStage() =>
        Ok(await _service.SimulateCurrentStageAsync());

    //[HttpPost("restart")]
    //public async Task<ActionResult<Tournament>> RestartTournament()
    //{
    //    var tournament = await _service.StartTournamentAsync();
    //    return Ok(tournament);
    //}

    // ✅ Get Current Tournament (for the UI)
    //[HttpGet]
    //public async Task<ActionResult<Tournament>> GetTournament()
    //{
    //    var result = await _service.GetActiveAsync();
    //    return Ok(result);
    //}


    [HttpPost("semifinals/simulate")]
    public async Task<IActionResult> SimulateSemiFinal(string homeId, string awayId)
    {
        var match = await _service.SimulateSemiFinalByIdAsync(homeId, awayId);
        if (match == null)
            return NotFound("Semi-final match not found or could not be simulated.");

        return Ok(match);
    }

    [HttpPost("restart")]
    public async Task<IActionResult> RestartTournament()
    {
        await _service.RestartTournamentAsync();
        return Ok(new { message = "Tournament restarted." });
    }

    [HttpGet("tournament")]
    public async Task<ActionResult<Tournament?>> GetTournament()
    {
        var tournament = await _service.GetOrCreateTournamentAsync();
        if (tournament == null)
            return NotFound("Not enough teams to start a tournament.");
        return Ok(tournament);
    }


}







