using SchoolManagement.Domain.Model;

namespace SchoolManagement.Domain.dto
{
    public class AddTeacherDto
    {
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }

        public Teacher ToModel()
        {
            return new()
            {
                Name = Name,
                Email = Email,
                PhoneNumber = PhoneNumber
            };
        }
    }
}
