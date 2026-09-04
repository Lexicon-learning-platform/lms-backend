using Lms_backend.Domain.Enums;

namespace Lms_backend.Infrastructure.Models;

public class ResourceSearchParams(string? name, string? search, ResourceType? type) : SearchParams(name, search)
{
    public ResourceType? Type { get; init; } = type;
}
