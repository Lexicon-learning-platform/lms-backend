using System.ComponentModel.DataAnnotations;
using Lms_backend.Domain.Enums;

namespace Lms_backend.Application.Models;

public class ResourceDto
{
    public Guid Id { get; set; }
    public UserSimpleDto CreatedBy { get; set; } = default!;
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public ResourceType Type { get; set; }
    public string Data { get; set; } = string.Empty;
}
