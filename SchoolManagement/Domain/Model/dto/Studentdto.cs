namespace SchoolManagement.dto
{
    public class StudentDto
    {
        public int Id { get; set; }
        public string? Name { get; set; } 
        public DateOnly DateOfBirth { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
    }
}