using AfricanNationsLeague.Application.Models;
using AfricanNationsLeague.Application.Services;
using Microsoft.AspNetCore.Mvc;


namespace AfricanNationsLeague.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserService _service;

        public UserController(UserService service)
        {
            _service = service;
        }

        //[HttpPost("register")]
        //public async Task<IActionResult> Register([FromBody] RegisterDto dto)
        //{
        //    try
        //    {
        //        var created = await _service.RegisterAsync(dto);
        //        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(new { message = ex.Message });
        //    }
        //}

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserDto dto)
        {
            try
            {
                await _service.RegisterAsync(dto);
                return Ok(new { message = "User registered successfully." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            try
            {
                var user = await _service.LoginAsync(dto);
                return Ok(user);
            }
            catch (Exception ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var users = await _service.GetAllAsync();
            return Ok(users);
        }

        //[HttpGet("{id}")]
        //public async Task<IActionResult> GetById(string id)
        //{
        //    var user = (await _service.GetAllAsync()).FirstOrDefault(u => u.Id == id);
        //    if (user == null)
        //        return NotFound();
        //    return Ok(user);
        //}

        [HttpGet("by-email")]
        public async Task<IActionResult> GetByEmail([FromQuery] string email)
        {
            var user = (await _service.GetAllAsync()).FirstOrDefault(u => u.Email == email);
            if (user == null)
                return NotFound();
            return Ok(user);
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            await _service.DeleteAsync(id);
            return NoContent();
        }
    }

}
