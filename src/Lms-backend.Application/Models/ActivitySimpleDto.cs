namespace Lms_backend.Application.Models;

public class ActivitySimpleDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public DateTime StartTime { get; set; }
    public int Duration { get; set; }
}
