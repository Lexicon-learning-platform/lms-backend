namespace Lms_backend.Application.Models;

public class CourseForChangeDto
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateOnly StartDate { get; set; }
    public int Duration { get; set; }
    public Guid[] ModuleIds { get; set; } = [];
}
