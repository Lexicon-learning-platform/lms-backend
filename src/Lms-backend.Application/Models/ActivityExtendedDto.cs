using Lms_backend.Domain.Enums;

namespace Lms_backend.Application.Models;

public class ActivityExtendedDto
{
    public Guid Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int StartOffset { get; set; }
    public int Duration { get; set; }
    public ActivityType Type { get; set; }
    public IEnumerable<ResourceSimpleDto> Resources { get; set; } = [];
}
