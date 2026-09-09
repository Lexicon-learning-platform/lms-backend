namespace Lms_backend.Application.Models;

public class ModuleExtendedDto
{
    public Guid Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int? StartOffset { get; set; }
    public int Duration { get; set; }
    public IEnumerable<ActivityDto> Activities { get; set; } = [];
    public IEnumerable<ResourceDto> Resources { get; set; } = [];
}
