using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchoolManagement.Data;
using SchoolManagement.Domain.Model;
using SchoolManagement.Domain.dto;

namespace SchoolManagement.Controllers
{
    [ApiController]
    [Route("api/admin")]
    [Authorize(Roles = "Admin")]
    public class AdminController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SchoolDbContext _context;

        public AdminController(
            UserManager<IdentityUser> userManager,
            SchoolDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        [HttpPost("assign-role")]
        public async Task<IActionResult> AssignRole([FromBody] AssignRoleDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.UserId) || string.IsNullOrWhiteSpace(dto.RoleName))
            {
                return BadRequest("UserId and RoleName are required.");
            }

            var validRoles = new[] { "Admin", "Teacher", "Student" };
            if (!validRoles.Contains(dto.RoleName))
            {
                return BadRequest($"Invalid role. Allowed roles: {string.Join(", ", validRoles)}");
            }

            var user = await _userManager.FindByIdAsync(dto.UserId);
            if (user == null)
            {
                return NotFound("User not found.");
            }

       
            var currentRoles = await _userManager.GetRolesAsync(user);
            if (currentRoles.Any())
            {
                var removeResult = await _userManager.RemoveFromRolesAsync(user, currentRoles);
                if (!removeResult.Succeeded)
                {
                    return BadRequest(removeResult.Errors);
                }
            }

   
            var addResult = await _userManager.AddToRoleAsync(user, dto.RoleName);
            if (!addResult.Succeeded)
            {
                return BadRequest(addResult.Errors);
            }

          
            if (dto.RoleName == "Teacher")
            {
      
                if (await _context.Teachers.AnyAsync(t => t.Email == user.Email))
                {
                    return Conflict("Teacher profile with this email already exists.");
                }

                var newTeacher = new Teacher
                {
                    Name = user.UserName ?? "New Teacher",
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber,
                 
                };

                _context.Teachers.Add(newTeacher);

                var student = await _context.Students.FirstOrDefaultAsync(s => s.Email == user.Email);
                if (student != null)
                {
                    _context.Students.Remove(student);
                }

                await _context.SaveChangesAsync();
            }

            return Ok(new 
            { 
                Message = $"Role '{dto.RoleName}' successfully assigned to user '{user.Email}'." 
            });
        }
    }
}