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

    [HttpPost("restart")]
    public async Task<ActionResult<Tournament>> RestartTournament()
    {
        var tournament = await _service.StartTournamentAsync();
        return Ok(tournament);
    }

    // ✅ Get Current Tournament (for the UI)
    [HttpGet]
    public async Task<ActionResult<Tournament>> GetTournament()
    {
        var result = await _service.GetActiveAsync();
        return Ok(result);
    }


    [HttpPost("semifinals/simulate")]
    public async Task<IActionResult> SimulateSemiFinal(string homeId, string awayId)
    {
        var match = await _service.SimulateSemiFinalByIdAsync(homeId, awayId);
        if (match == null)
            return NotFound("Semi-final match not found or could not be simulated.");

        return Ok(match);
    }
}








//[ApiController]
//[Route("api/[controller]")]
//public class TournamentController : ControllerBase
//{
//    private readonly TournamentService _service;

//    public TournamentController(TournamentService service)
//    {
//        _service = service;
//    }

//    [HttpGet("active")]
//    public async Task<IActionResult> GetActive()
//    {
//        var t = await _service.GetActiveAsync();
//        return Ok(t);
//    }

//    [HttpPost("start")]
//    public async Task<IActionResult> Start()
//    {
//        // you already have your start logic; keep it
//        // just return the created tournament
//        var t = await _service.StartTournamentAsync(); // your existing method
//        return Ok(t);
//    }

//    // NEW: simulate the CURRENT stage (Quarterfinal, then Semifinal, then Final)
//    [HttpPost("simulate-stage")]
//    public async Task<IActionResult> SimulateStage()
//    {
//        var t = await _service.SimulateCurrentStageAsync();
//        return Ok(t);
//    }
//}
