using AfricanNationsLeague.Application.Models;
using AfricanNationsLeague.Domain.Entities;
using AfricanNationsLeague.Infrastructure.Interface;

namespace AfricanNationsLeague.Application.Services
{
    public class UserService
    {
        private readonly IUserRepository _repo;

        public UserService(IUserRepository repo)
        {
            _repo = repo;
        }

        public async Task RegisterAsync(UserDto dto)
        {
            // Check if email already exists
            var existing = await _repo.GetByEmailAsync(dto.Email);
            if (existing != null)
                throw new Exception("Email already registered.");

            var passwordHash = BCrypt.Net.BCrypt.HashPassword(dto.PasswordHash);

            var user = new User
            {
                FullName = dto.FullName,
                Email = dto.Email,
                PasswordHash = passwordHash,
                Role = dto.Role,
                Country = new Country
                {
                    Code = dto.Country.Code,
                    Name = dto.Country.Name,
                    FlagUrl = dto.Country.FlagUrl
                },
                CreatedAt = DateTime.UtcNow

            };

            await _repo.AddAsync(user);


        }

        public async Task<UserDto?> LoginAsync(LoginDto dto)
        {
            var user = await _repo.GetByEmailAsync(dto.Email);
            if (user == null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
                throw new Exception("Invalid email or password.");

            var passwordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password);
            return new UserDto
            {

                FullName = user.FullName,
                Email = user.Email,
                PasswordHash = passwordHash,
                Role = user.Role,
                Country = user.Country,
                CreatedAt = user.CreatedAt

            };
        }

        public async Task<IEnumerable<UserDto>> GetAllAsync()
        {

            var users = await _repo.GetAllAsync();
            return users.Select(u => new UserDto
            {
                //Id = u.Id,
                FullName = u.FullName,
                Email = u.Email,
                Role = u.Role,
                PasswordHash = u.PasswordHash,
                Country = u.Country,
                CreatedAt = u.CreatedAt

            });
        }

        public async Task DeleteAsync(string id)
        {
            await _repo.DeleteAsync(id);
        }
    }
}


