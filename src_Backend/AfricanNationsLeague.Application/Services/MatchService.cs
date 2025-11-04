using AfricanNationsLeague.Application.Models;
using AfricanNationsLeague.Domain.Entities;
using AfricanNationsLeague.Infrastructure.Interface;

namespace AfricanNationsLeague.Application.Services
{
    public class MatchService
    {
        private readonly IMatchRepository _matchRepository;
        private readonly ITeamRepository _teamRepository;
        private readonly Random _random = new();

        public MatchService(IMatchRepository matchRepository, ITeamRepository teamRepository)
        {
            _matchRepository = matchRepository;
            _teamRepository = teamRepository;
        }

        // ================================
        // 1️⃣ Simulate a match
        // ================================

        public async Task<Match> SimulateMatchAsync(string homeTeamId, string awayTeamId, string stage)
        {
            var homeTeam = await _teamRepository.GetByIdAsync(homeTeamId);
            var awayTeam = await _teamRepository.GetByIdAsync(awayTeamId);

            if (homeTeam == null || awayTeam == null)
                throw new Exception("Both teams must exist to simulate a match.");

            double ratingSum = homeTeam.AverageRating + awayTeam.AverageRating;
            double homeProbability = homeTeam.AverageRating / ratingSum;

            int homeScore = _random.Next(0, 4);
            int awayScore = _random.Next(0, 4);

            if (_random.NextDouble() < homeProbability)
                homeScore++;
            else
                awayScore++;

            var homeGoals = GenerateGoals(homeTeam, homeScore);
            var awayGoals = GenerateGoals(awayTeam, awayScore);

            string winnerTeamId = homeScore > awayScore ? homeTeam.Id
                               : awayScore > homeScore ? awayTeam.Id
                               : "draw";

            var commentary = new List<CommentaryEvent>();

            commentary.Add(new CommentaryEvent { Minute = 0, Text = $"Kickoff! {homeTeam.Country.Name} vs {awayTeam.Country.Name}" });

            foreach (var goal in homeGoals)
                commentary.Add(new CommentaryEvent { Minute = goal.Minute, Text = $"{homeTeam.Country.Name} scores! {goal.PlayerName}!" });

            foreach (var goal in awayGoals)
                commentary.Add(new CommentaryEvent { Minute = goal.Minute, Text = $"{awayTeam.Country.Name} scores! {goal.PlayerName}!" });

            commentary.Add(new CommentaryEvent { Minute = 90, Text = "Full-time whistle! What a match!" });

            var match = new Match
            {

                HomeTeamId = homeTeam.Id,
                AwayTeamId = awayTeam.Id,

                HomeCountry = homeTeam.Country,
                AwayCountry = awayTeam.Country,

                HomeScore = homeScore,
                AwayScore = awayScore,
                HomeGoals = homeGoals,
                AwayGoals = awayGoals,

                Commentary = commentary,

                WinnerCountryCode = winnerTeamId == "draw" ? "draw" : homeTeam.Country.Code,

                Stage = stage,
                PlayedAt = DateTime.UtcNow,
                IsPlayed = true
            };

            await _matchRepository.AddAsync(match);
            return match;
        }

        // ================================
        // 2️⃣ Get all matches
        // ================================


        public async Task<List<MatchDto>> GetAllAsync()
        {
            var matches = await _matchRepository.GetAllAsync();

            return matches.Select(m => new MatchDto
            {
                Id = m.Id,
                HomeTeamId = m.HomeTeamId,
                AwayTeamId = m.AwayTeamId,

                HomeCountry = new CountriesDto
                {
                    Code = m.HomeCountry.Code,
                    Name = m.HomeCountry.Name,
                    FlagUrl = m.HomeCountry.FlagUrl
                },
                AwayCountry = new CountriesDto
                {
                    Code = m.AwayCountry.Code,
                    Name = m.AwayCountry.Name,
                    FlagUrl = m.AwayCountry.FlagUrl
                },

                HomeScore = m.HomeScore,
                AwayScore = m.AwayScore,
                IsPlayed = m.IsPlayed,
                Stage = m.Stage,
                PlayedAt = m.PlayedAt,
                Status = m.IsPlayed ? "Completed" : "Pending"
            }).ToList();
        }


        // ================================
        // 3️⃣ Get match by ID
        // ================================
        public async Task<Match?> GetByIdAsync(string id)
        {
            return await _matchRepository.GetByIdAsync(id);
        }

        // ================================
        // 4️⃣ Helper: Generate goal scorers
        // ================================

        private List<Goal> GenerateGoals(Team team, int goals)
        {
            var result = new List<Goal>();
            for (int i = 0; i < goals; i++)
            {
                var scorer = team.Players[_random.Next(team.Players.Count)];
                result.Add(new Goal
                {
                    PlayerName = scorer.Name,
                    Minute = _random.Next(1, 91)
                });
            }
            return result.OrderBy(g => g.Minute).ToList();
        }


        // ✅ 1️⃣ Simulate a match by MatchId (Admin Action)
        public async Task<Match> SimulateMatchByIdAsync(string Id)
        {
            var match = await _matchRepository.GetByIdAsync(Id);
            if (match == null) throw new Exception($"Match {Id} not found.");


            return await SimulateMatchAsync(match.HomeTeamId, match.AwayTeamId, match.Stage);
        }


        // ✅ 2️⃣ Play a match manually by MatchId (Replay fresh match)
        public async Task<Match> PlayMatchByIdAsync(string matchId)
        {
            var match = await _matchRepository.GetByIdAsync(matchId);
            if (match == null) throw new Exception($"Match {matchId} not found.");

            // ✅ Reuse your existing simulation logic until manual logic is created
            return await SimulateMatchAsync(match.HomeTeamId, match.AwayTeamId, match.Stage);
        }







        // ================================
        // 5️⃣ Helper: Generate commentary
        // ================================
        private string GenerateCommentary(string home, string away, int hs, int ascore)
        {
            if (hs > ascore)
                return $"{home} outplayed {away} with a {hs}-{ascore} victory.";
            if (hs < ascore)
                return $"{away} narrowly beat {home} {ascore}-{hs}.";
            return $"{home} and {away} played to a tense {hs}-{ascore} draw.";
        }
    }
}
