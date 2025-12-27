using SchoolManagement.Domain.Model;

namespace SchoolManagement.Domain.dto
{
    public class AddStudentDto
    {

        public string? Name { get; set; } 
        public DateOnly DateOfBirth { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }

        public Student ToModel()
        {
            return new()
            {
     
                Name=Name,
                DateOfBirth=DateOfBirth,
                Email=Email,
                PhoneNumber=PhoneNumber
            };
        }
    }
}