using Microsoft.AspNetCore.Identity;

namespace SchoolManagement.Service
{
    public interface IAuthService
{
    Task<string> LoginAsync(string email, string password);
    Task RegisterAsync(string userName, string email, string password, string? phoneNumber);
}
} 