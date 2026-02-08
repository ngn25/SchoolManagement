using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using SchoolManagement.Domain.dto;
using SchoolManagement.Service;

namespace SchoolManagement.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto dto)
        {
            try
            {
                await _authService.RegisterAsync(dto.UserName, dto.Email, dto.Password, dto.PhoneNumber);
                return Ok(new { message = "User registered successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            try
            {
                var token = await _authService.LoginAsync(dto.Email, dto.Password);
                return Ok(new { token });
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized(new { message = "Invalid email or password" });
            }
        }
    }
}


