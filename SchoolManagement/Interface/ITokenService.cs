using Microsoft.AspNetCore.Identity;

namespace SchoolManagement.Service
{
    public interface ITokenService
    {
        string GenerateToken(IdentityUser user, IList<string> roles = null);
    }
}