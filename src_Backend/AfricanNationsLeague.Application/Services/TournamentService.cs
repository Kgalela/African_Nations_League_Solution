using AfricanNationsLeague.Application.Services;
using AfricanNationsLeague.Domain.Entities;
using AfricanNationsLeague.Infrastructure.Interface;


public class TournamentService
{
    private readonly ITournamentRepository _tournamentRepo;
    private readonly ITeamRepository _teamRepo;
    private readonly MatchService _matchService;
    private readonly Random _rng = new();

    public TournamentService(ITournamentRepository tournamentRepo, ITeamRepository teamRepo, MatchService matchService)
    {
        _tournamentRepo = tournamentRepo;
        _teamRepo = teamRepo;
        _matchService = matchService;
    }

    //public async Task<Tournament> StartTournamentAsync()
    //{
    //    var teams = (await _teamRepo.GetAllAsync()).Take(8).ToList();
    //    if (teams.Count < 7)
    //        throw new Exception("Tournament requires at least 7 teams registered.");

    //    var matches = new List<Match>();

    //    // Create matches for available teams
    //    for (int i = 0; i < teams.Count - 1; i += 2)
    //    {
    //        if (i + 1 < teams.Count)
    //        {
    //            matches.Add(new Match
    //            {
    //                HomeTeamId = teams[i].Id,
    //                AwayTeamId = teams[i + 1].Id,
    //                HomeCountry = teams[i].Country,
    //                AwayCountry = teams[i + 1].Country,
    //                Stage = "Quarterfinal",
    //                IsPlayed = false
    //            });
    //        }
    //        else
    //        {
    //            // Odd team out, create a match with a placeholder for the 8th team
    //            matches.Add(new Match
    //            {
    //                HomeTeamId = teams[i].Id,
    //                AwayTeamId = null,
    //                HomeCountry = teams[i].Country,
    //                AwayCountry = null,
    //                Stage = "Quarterfinal",
    //                IsPlayed = false
    //            });
    //        }
    //    }

    //    var tournament = new Tournament
    //    {
    //        Name = "African Nations League 2026",
    //        Matches = matches,
    //        CurrentStage = "Quarterfinal",
    //        IsCompleted = false,
    //        CreatedAt = DateTime.UtcNow
    //    };

    //    await _tournamentRepo.AddAsync(tournament);
    //    return tournament;
    //}


    public async Task<Tournament> StartTournamentAsync()
    {
        var teams = (await _teamRepo.GetAllAsync()).Take(8).ToList();
        if (teams.Count != 8)
            throw new Exception("Tournament requires exactly 8 teams registered.");

        var matches = new List<Match>
    {
        new Match {

            HomeTeamId = teams[0].Id,
            AwayTeamId = teams[1].Id,
            HomeCountry = teams[0].Country,
            AwayCountry = teams[1].Country,
            Stage = "Quarterfinal",
            IsPlayed = false

        },
        new Match {


            HomeTeamId = teams[2].Id,
            AwayTeamId = teams[3].Id,
            HomeCountry = teams[2].Country,
            AwayCountry = teams[3].Country,
            Stage = "Quarterfinal",
            IsPlayed = false
        },
        new Match {


            HomeTeamId = teams[4].Id,
            AwayTeamId = teams[5].Id,
            HomeCountry = teams[4].Country,
            AwayCountry = teams[5].Country,
            Stage = "Quarterfinal",
            IsPlayed = false
        },
        new Match {


            HomeTeamId = teams[6].Id,
            AwayTeamId = teams[7].Id,
            HomeCountry = teams[6].Country,
            AwayCountry = teams[7].Country,
            Stage = "Quarterfinal",
            IsPlayed = false
        }
    };



        var tournament = new Tournament
        {
            Name = "African Nations League 2026",

            Matches = matches,

            CurrentStage = "Quarterfinal",
            IsCompleted = false,
            CreatedAt = DateTime.UtcNow
        };

        await _tournamentRepo.AddAsync(tournament);
        return tournament;
    }

    public async Task<Tournament?> GetOrCreateTournamentAsync()
    {
        // Get the latest tournament (active or not)
        var tournaments = (await _tournamentRepo.GetAllAsync()).OrderByDescending(t => t.CreatedAt).ToList();
        var tournament = tournaments.FirstOrDefault();

        if (tournament != null && !tournament.IsCompleted)
            return tournament;

        var teams = (await _teamRepo.GetAllAsync()).ToList();
        if (teams.Count >= 8)
        {
            // Start and return a new tournament if there are at least 8 teams
            await StartTournamentAsync();
            return await SimulateCurrentStageAsync();
        }

        // Not enough teams to start a tournament
        return null;
    }





    // Get latest tournament (active)
    public async Task<Tournament?> GetActiveAsync()
    {
        var all = (await _tournamentRepo.GetAllAsync()).OrderByDescending(t => t.CreatedAt).ToList();
        return all.FirstOrDefault();
    }




    // Start tournament (you already have similar code). Keep yours.
    // IMPORTANT: make sure you pick 8 teams and create 4 Quarterfinal matches.

    // ===== NEW: simulate the current stage and advance =====
    public async Task<Tournament> SimulateCurrentStageAsync()
    {
        var t = await GetActiveAsync() ?? throw new Exception("No active tournament.");
        var stage = t.CurrentStage; // "Quarterfinal" | "Semifinal" | "Final" | "Completed"
        if (stage == "Completed") return t;

        // 1) simulate all unplayed matches in this stage
        var stageMatches = t.Matches.Where(m => m.Stage == stage && !m.IsPlayed).ToList();
        if (!stageMatches.Any())
        {
            // nothing to simulate, but we can try to advance if already played
            await AdvanceIfStageCompleteAsync(t, stage);
            return t;
        }

        foreach (var m in stageMatches)
        {
            // We stored Country objects in matches. Those Country objects include an Id field in your JSON.
            // Use that Id to get the Team and pass to MatchService (which expects team IDs).
            var homeTeam = await _teamRepo.GetByIdAsync(m.HomeTeamId);
            var awayTeam = await _teamRepo.GetByIdAsync(m.AwayTeamId);

            if (homeTeam == null || awayTeam == null)
                throw new Exception($"Team(s) not found for match simulation. HomeTeamId={m.HomeTeamId}, AwayTeamId={m.AwayTeamId}");


            var result = await _matchService.SimulateMatchAsync(homeTeam.Id, awayTeam.Id, m.Stage);

            // copy result into tournament match
            m.HomeScore = result.HomeScore;
            m.AwayScore = result.AwayScore;
            m.HomeGoals = result.HomeGoals;
            m.AwayGoals = result.AwayGoals;

            // winnerCountryCode comes from winner team
            if (!string.IsNullOrEmpty(result.WinnerCountryCode) && result.WinnerCountryCode != "draw")
            {
                if (result.WinnerCountryCode == homeTeam.Id) m.WinnerCountryCode = homeTeam.Country.Code;
                else if (result.WinnerCountryCode == awayTeam.Id) m.WinnerCountryCode = awayTeam.Country.Code;
            }

            m.IsPlayed = true;
            m.PlayedAt = DateTime.UtcNow;
        }

        await _tournamentRepo.UpdateAsync(t);

        // 2) if whole stage finished, create next stage with random reseeding
        await AdvanceIfStageCompleteAsync(t, stage);

        return t;
    }


    public async Task<Match?> SimulateSemiFinalByIdAsync(string homeId, string awayId)
    {
        var tournament = await GetActiveAsync();
        if (tournament == null)
            throw new Exception("No active tournament.");

        // Find the semi-final match by ID
        var match = tournament.Matches.FirstOrDefault(m => m.HomeTeamId == homeId && m.AwayTeamId == awayId && m.Stage == "Semifinal");
        if (match == null)
            throw new Exception("Semi-final match not found.");

        // Get teams
        var homeTeam = await _teamRepo.GetByIdAsync(match.HomeTeamId);
        var awayTeam = await _teamRepo.GetByIdAsync(match.AwayTeamId);

        if (homeTeam == null || awayTeam == null)
            throw new Exception("Teams not found for this semi-final match.");

        // Simulate the match
        var result = await _matchService.SimulateMatchAsync(homeTeam.Id, awayTeam.Id, match.Stage);

        // Update match details
        match.HomeScore = result.HomeScore;
        match.AwayScore = result.AwayScore;
        match.HomeGoals = result.HomeGoals;
        match.AwayGoals = result.AwayGoals;
        match.Commentary = result.Commentary;
        match.WinnerCountryCode = result.WinnerCountryCode;
        match.IsPlayed = true;
        match.PlayedAt = DateTime.UtcNow;



        await _tournamentRepo.UpdateAsync(tournament);

        return match;
    }




    private async Task AdvanceIfStageCompleteAsync(Tournament t, string stageJustPlayed)
    {
        var allPlayed = t.Matches.Where(x => x.Stage == stageJustPlayed).All(x => x.IsPlayed);
        if (!allPlayed)
        {
            await _tournamentRepo.UpdateAsync(t);
            return;
        }

        // Get all teams for lookup
        var allTeams = (await _teamRepo.GetAllAsync()).ToList();

        if (stageJustPlayed == "Quarterfinal")
        {
            var winners = WinnersAsCountries(t, "Quarterfinal");
            Shuffle(winners);

            var homeTeamA = allTeams.FirstOrDefault(x => x.Country.Code == winners[0].Code);
            var awayTeamA = allTeams.FirstOrDefault(x => x.Country.Code == winners[1].Code);
            var homeTeamB = allTeams.FirstOrDefault(x => x.Country.Code == winners[2].Code);
            var awayTeamB = allTeams.FirstOrDefault(x => x.Country.Code == winners[3].Code);

            t.Matches.Add(new Match
            {
                HomeTeamId = homeTeamA?.Id ?? "",
                AwayTeamId = awayTeamA?.Id ?? "",
                HomeCountry = winners[0],
                AwayCountry = winners[1],
                Stage = "Semifinal",
                IsPlayed = false

            });
            t.Matches.Add(new Match
            {
                HomeTeamId = homeTeamB?.Id ?? "",
                AwayTeamId = awayTeamB?.Id ?? "",
                HomeCountry = winners[2],
                AwayCountry = winners[3],
                Stage = "Semifinal",
                IsPlayed = false
            });
            t.CurrentStage = "Semifinal";
        }
        else if (stageJustPlayed == "Semifinal")
        {
            var winners = WinnersAsCountries(t, "Semifinal");
            Shuffle(winners);

            var homeTeam = allTeams.FirstOrDefault(x => x.Country.Code == winners[0].Code);
            var awayTeam = allTeams.FirstOrDefault(x => x.Country.Code == winners[1].Code);

            t.Matches.Add(new Match
            {
                HomeTeamId = homeTeam?.Id ?? "",
                AwayTeamId = awayTeam?.Id ?? "",
                HomeCountry = winners[0],
                AwayCountry = winners[1],
                Stage = "Final",
                IsPlayed = false
            });
            t.CurrentStage = "Final";
        }
        else if (stageJustPlayed == "Final")
        {
            t.CurrentStage = "Completed";
            t.IsCompleted = true;
        }

        await _tournamentRepo.UpdateAsync(t);
    }




    public async Task<List<Match>> GetSemiFinalMatchesAsync()
    {
        var tournament = await GetActiveAsync();
        if (tournament == null) return new List<Match>();

        return tournament.Matches
            .Where(m => m.Stage == "Semifinal")
            .ToList();
    }


    private List<Country> WinnersAsCountries(Tournament t, string stage)
    {
        var list = new List<Country>();
        var matches = t.Matches.Where(m => m.Stage == stage).ToList();
        foreach (var m in matches)
        {
            // Prefer code match against the countries present in the match
            if (!string.IsNullOrEmpty(m.WinnerCountryCode))
            {
                if (m.HomeCountry.Code == m.WinnerCountryCode) list.Add(m.HomeCountry);
                else if (m.AwayCountry.Code == m.WinnerCountryCode) list.Add(m.AwayCountry);
            }
            else
            {
                // fallback: pick higher score if no code
                if (m.HomeScore >= m.AwayScore) list.Add(m.HomeCountry);
                else list.Add(m.AwayCountry);
            }
        }
        return list;
    }

    private void Shuffle<T>(IList<T> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int j = _rng.Next(i + 1);
            (list[i], list[j]) = (list[j], list[i]);
        }
    }


    public async Task OnTeamRegisteredAsync(Team newTeam)
    {
        // get latest tournament or create one
        var t = (await _tournamentRepo.GetAllAsync())
            .OrderByDescending(x => x.CreatedAt)
            .FirstOrDefault();

        if (t == null || t.IsCompleted)
        {
            t = new Tournament
            {
                Name = "African Nations League 2026",
                Matches = new List<Match>(),
                CurrentStage = "Quarterfinal",
                IsCompleted = false,
                CreatedAt = DateTime.UtcNow
            };
            await _tournamentRepo.AddAsync(t);
        }

        // if this team’s country is already in the bracket, skip
        var alreadyThere = t.Matches
            .SelectMany(m => new[] { m.HomeCountry?.Code, m.AwayCountry?.Code })
            .Where(c => !string.IsNullOrEmpty(c))
            .Distinct()
            .Contains(newTeam.Country.Code);

        if (!alreadyThere && t.CurrentStage == "Quarterfinal" && !t.IsCompleted)
        {
            // collect distinct countries already present in quarterfinals
            var existingCodes = t.Matches
                .Where(m => m.Stage == "Quarterfinal")
.SelectMany(m => new[] { m.HomeCountry?.Code, m.AwayCountry?.Code }.Where(c => c != null))
                .Distinct()
                .ToHashSet();

            // how many slots are used
            var used = existingCodes.Count;
            if (used < 8)
            {
                // we will temporarily hold the country until we can form pairs of two
                // try to find a pending match with an empty slot
                var pending = t.Matches
                    .FirstOrDefault(m => m.Stage == "Quarterfinal" &&
                                         (m.HomeCountry == null || m.AwayCountry == null));

                if (pending == null)
                {
                    // start a new match and place team as Home
                    t.Matches.Add(new Match
                    {
                        HomeTeamId = newTeam.Id,
                        AwayTeamId = newTeam.Id,
                        HomeCountry = newTeam.Country,
                        Stage = "Quarterfinal",
                        IsPlayed = false,
                        PlayedAt = DateTime.UtcNow
                    });
                }
                else
                {
                    // fill the empty side
                    if (pending.HomeCountry == null) pending.HomeCountry = newTeam.Country;
                    else if (pending.AwayCountry == null) pending.AwayCountry = newTeam.Country;
                }

                // Once we reach 8 unique country slots, make sure there are exactly 4 full matches
                existingCodes = t.Matches
                    .Where(m => m.Stage == "Quarterfinal")
                    .SelectMany(m => new[] { m.HomeCountry?.Code, m.AwayCountry?.Code })
                    .Where(c => !string.IsNullOrEmpty(c))
                    .Distinct()
                    .ToHashSet();

                if (existingCodes.Count == 8)
                {
                    // ensure four fully defined matches, no nulls
                    var qf = t.Matches.Where(m => m.Stage == "Quarterfinal").ToList();
                    // remove any incomplete matches
                    qf.RemoveAll(m => m.HomeCountry == null || m.AwayCountry == null);
                    t.Matches = t.Matches.Where(m => m.Stage != "Quarterfinal").ToList();
                    t.Matches.AddRange(qf);
                }
            }

            await _tournamentRepo.UpdateAsync(t);
        }
    }

    public async Task RestartTournamentAsync()
    {
        // Delete all tournaments
        var allTournaments = await _tournamentRepo.GetAllAsync();
        foreach (var t in allTournaments)
        {
            await _tournamentRepo.DeleteAsync(t.Id);
        }

        // Optionally, clear all matches if stored separately
        // Uncomment if you want to clear all matches from the match repository
        //if (_matchService != null && _matchService.GetAllAsync != null)
        //{
        //    var allMatches = await _matchService.GetAllAsync();
        //    foreach (var m in allMatches)
        //    {
        //        await _matchService.DeleteAsync(m.Id);
        //    }
        //}

        // Start a new tournament
        await StartTournamentAsync();
    }



}
