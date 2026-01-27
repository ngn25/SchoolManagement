using Microsoft.AspNetCore.Mvc;           
using System.Threading.Tasks;            
using SchoolManagement.Domain.dto;       
using SchoolManagement.Service; 
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
        await _authService.RegisterAsync(dto.UserName, dto.Email, dto.Password, dto.PhoneNumber);
        return Ok("User registered successfully");
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDto dto)
    {
        var token = await _authService.LoginAsync(dto.Email, dto.Password);
        return Ok(new { token });
    }
}

