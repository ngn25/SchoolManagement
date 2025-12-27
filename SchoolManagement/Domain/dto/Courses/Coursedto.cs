using SchoolManagement.Domain.Model;

namespace SchoolManagement.Domain.dto
{
    public class CourseDto
    {
        public int? Id { get; set; }

        public string? Name { get; set; }

        public int TeacherId { get; set; }
    

        public Course ToModel()
        {
            return new()
            {
                Id = Id,
                Name = Name,
                TeacherId = TeacherId
            };
        }
    }
}
