using AfricanNationsLeague.Application.Models;
using AfricanNationsLeague.Domain.Entities;
using AfricanNationsLeague.Infrastructure.Interface;

namespace AfricanNationsLeague.Application.Services
{
    public class TeamService
    {
        private readonly ITeamRepository _repo;
        private readonly Random _random = new();
        private readonly FootballApiService _footballApi;
        private readonly TournamentService _tournamentService;

        public TeamService(ITeamRepository repo, FootballApiService footballApi, TournamentService tournamentService)
        {
            _repo = repo;
            _footballApi = footballApi;
            _tournamentService = tournamentService;
        }

        public async Task<TeamDto> CreateTeamAsync(CreateTeamDto dto)
        {
            var players = GeneratePlayers();

            //var players = await GeneratePlayers(dto.Country.Name);


            var average = players.Average(p => p.Ratings[p.NaturalPosition]);

            var team = new Team
            {
                Country = new Country
                {
                    Code = dto.Country.Code,
                    Name = dto.Country.Name,
                    FlagUrl = dto.Country.FlagUrl
                },
                ManagerName = dto.ManagerName,
                email = dto.email,
                Players = players,
                AverageRating = Math.Round(average, 2),
                CreatedAt = DateTime.UtcNow
            };

            var savedTeam = await _repo.AddAsync(team);
            await _tournamentService.OnTeamRegisteredAsync(savedTeam);

            return new TeamDto
            {
                Id = savedTeam.Id,
                Country = savedTeam.Country,
                ManagerName = savedTeam.ManagerName,
                email = savedTeam.email,
                AverageRating = savedTeam.AverageRating,
                Players = savedTeam.Players,
                CreatedAt = savedTeam.CreatedAt
            };
        }

        //private async Task<List<Player>> GeneratePlayers(string countryName)
        //{
        //    var positions = new List<string>();
        //    positions.AddRange(Enumerable.Repeat("GK", 3));
        //    positions.AddRange(Enumerable.Repeat("DF", 7));
        //    positions.AddRange(Enumerable.Repeat("MD", 8));
        //    positions.AddRange(Enumerable.Repeat("AT", 5));

        //    var random = new Random();

        //    Fetch real players(fallback if empty)
        //        var realNames = await _footballApi.GetPlayersByCountryAsync(countryName);
        //    if (!realNames.Any())
        //    {
        //        realNames = Enumerable.Range(1, 23)
        //            .Select(i => $"Player_{i}")
        //            .ToList();
        //    }

        //    var players = new List<Player>();

        //    foreach (var pos in positions)
        //    {
        //        var nameIndex = random.Next(realNames.Count);
        //        var name = realNames[nameIndex];
        //        realNames.RemoveAt(nameIndex);

        //        var player = new Player
        //        {
        //            Name = name,
        //            NaturalPosition = pos,
        //            Ratings = new Dictionary<string, int>()
        //        };

        //        foreach (var p in new[] { "GK", "DF", "MD", "AT" })
        //        {
        //            player.Ratings[p] = (p == pos)
        //                ? random.Next(50, 101)
        //                : random.Next(20, 60);
        //        }

        //        players.Add(player);
        //    }

        //    players[0].IsCaptain = true;
        //    return players;
        //}


        private List<Player> GeneratePlayers()
        {
            var players = new List<Player>();
            var random = new Random();

            var positions = new List<string>();
            positions.AddRange(Enumerable.Repeat("GK", 3));
            positions.AddRange(Enumerable.Repeat("DF", 7));
            positions.AddRange(Enumerable.Repeat("MD", 8));
            positions.AddRange(Enumerable.Repeat("AT", 5));



            foreach (var pos in positions)
            {
                var player = new Player
                {
                    Name = $"Player_{Guid.NewGuid().ToString()[..5]}",
                    NaturalPosition = pos,
                    Ratings = new Dictionary<string, int>()
                };

                foreach (var p in new[] { "GK", "DF", "MD", "AT" })
                {
                    player.Ratings[p] = (p == pos)
                        ? random.Next(50, 101)
                        : random.Next(0, 51);
                }

                players.Add(player);
            }

            players[0].IsCaptain = true;
            return players;
        }


        public async Task<IEnumerable<TeamDto>> GetAllAsync()
        {
            var teams = await _repo.GetAllAsync();
            return teams.Select(t => new TeamDto
            {
                Id = t.Id,
                Country = t.Country,
                ManagerName = t.ManagerName,
                email = t.email,
                Players = t.Players,
                AverageRating = t.AverageRating,
                CreatedAt = t.CreatedAt
            });
        }

        public async Task<TeamDto?> GetByIdAsync(string id)
        {
            var team = await _repo.GetByIdAsync(id);
            if (team == null) return null;

            return new TeamDto
            {
                Id = team.Id,
                Country = team.Country,
                ManagerName = team.ManagerName,
                email = team.email,
                Players = team.Players,
                AverageRating = team.AverageRating,
                CreatedAt = team.CreatedAt
            };
        }

        public async Task DeleteAsync(string id)
        {
            await _repo.DeleteAsync(id);
        }
    }
}
