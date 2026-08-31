namespace Lms_backend.Application.Models;

public class ModuleForChangeDto
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateOnly StartDate { get; set; }
    public int Duration { get; set; }
    public Guid[] ActivityIds { get; set; } = [];
}
