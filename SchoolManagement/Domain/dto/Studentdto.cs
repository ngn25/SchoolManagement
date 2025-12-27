using SchoolManagement.Domain.Model;

namespace SchoolManagement.Domain.dto
{
    public class StudentDto
    {
        public int? Id { get; set; }
        public string? Name { get; set; } 
        public DateOnly DateOfBirth { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }

        public Student ToModel()
        {
            return new()
            {
                Id=Id,
                Name=Name,
                DateOfBirth=DateOfBirth,
                Email=Email,
                PhoneNumber=PhoneNumber
            };
        }
    }
}