using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
namespace SchoolManagement.Service
{
public class AuthService : IAuthService
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly IConfiguration _configuration;

    public AuthService
    (
        UserManager<IdentityUser> userManager,
        SignInManager<IdentityUser> signInManager,
        IConfiguration configuration
    )
    
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _configuration = configuration;
    }
    public async Task<string> LoginAsync(string email, string password)
    {

        var user = await _userManager.FindByEmailAsync(email);
        if (user == null)
            throw new UnauthorizedAccessException("Invalid credentials");

        var result = await _signInManager.CheckPasswordSignInAsync(user, password, false);
        if (!result.Succeeded)
            throw new UnauthorizedAccessException("Invalid credentials");

        return GenerateToken(user);
    }

    public async Task RegisterAsync(string userName, string email, string password, string? phoneNumber)
    {
        var exists = await _userManager.FindByEmailAsync(email);
        if (exists != null)
            throw new Exception("User already exists");

        var user = new IdentityUser
        {
            UserName = userName,
            Email = email,
            PhoneNumber = phoneNumber
        };

        var result = await _userManager.CreateAsync(user, password);

        if (!result.Succeeded)
            throw new Exception(string.Join(", ", result.Errors.Select(e => e.Description)));
    }


    private string GenerateToken(IdentityUser user)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id),
            new Claim(ClaimTypes.Name, user.UserName!),
            new Claim(ClaimTypes.Email, user.Email!),
            new Claim(ClaimTypes.MobilePhone,user.PhoneNumber!)
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Key"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _configuration["JWT:Issuer"],
            audience: _configuration["JWT:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(20),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}


}
