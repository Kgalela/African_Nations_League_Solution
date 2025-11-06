using Web.Infrustructure.Models;

namespace Web.Infrustructure.Services
{
    public interface IAfricanNationsLeagueApi
    {
        Task RegisterUser(UserDto user);

        Task<UserDto?> LoginUser(LoginDto login);

        Task<List<Country>> GetCountriesFlags();
        Task<List<TeamDto>> GetTeams();
        Task<TeamDto> RegisterTeam(CreateTeamDto team);
        Task<TournamentBracketDto?> GetTournamentAsync();
        Task<TournamentBracketDto?> StartTournamentAsync();
        Task<TournamentBracketDto?> SimulateStageAsync();
        Task<TournamentBracketDto?> RestartTournamentAsync();
        Task<TournamentBracketDto?> SimulateStageSemiFinalsAsync(string homeId, string awayId);
        Task<UserDto?> GetUserByEmail(string email);

        Task<MatchDto> GetMatchByID(string id);
        Task SendEmail(SendEmailRequestDto sendEmailRequest);
        Task<List<MatchDto>> GetAllMatches();

        Task<MatchDto> MathchSimulateMatch(string homeTeamId, string awayTeamId, string stage);

        //Task<MatchDto> PlayMatchById(string matchId);

        //Task<MatchDto> SimulateMatchAsync(string homeTeamId, string awayTeamId, string stage);
        //Task<List<MatchDto>> GetAllMatchesAsync();
        //Task<MatchDto?> GetMatchByIdAsync(string id);
    }
}
