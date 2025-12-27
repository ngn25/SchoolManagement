using SchoolManagement.Domain.Model;

namespace SchoolManagement.Domain.dto
{
    public class AddCourseDto
    {
     
        public string? Name { get; set; }

        public int TeacherId { get; set; }
    

        public Course ToModel()
        {
            return new()
            {
                Name = Name,
                TeacherId = TeacherId
            };
        }
    }
}
