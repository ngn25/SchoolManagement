using SchoolManagement.Domain.Model;

namespace SchoolManagement.Domain.dto
{
    public class TeacherDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }

        public Teacher ToModel()
        {
            return new()
            {
                Id = Id,
                Name = Name,
                Email = Email,
                PhoneNumber = PhoneNumber
            };
        }
    }
}
