using Lms_backend.Domain.Enums;

namespace Lms_backend.Infrastructure.Models;

public class ActivitySearchParams(string? name, string? search, ActivityType? type) : SearchParams(name, search)
{
    public ActivityType? Type { get; init; } = type;
}
