namespace Lms_backend.Application.Models;

public class UserExtendedDto
{
    public Guid Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public string GivenName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public CourseSimpleDto[] Courses { get; set; } = [];
}
