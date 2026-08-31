namespace Lms_backend.Application.Models;

public class ActivityForChangeDto
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime StartTime { get; set; }
    public int Duration { get; set; }
}
