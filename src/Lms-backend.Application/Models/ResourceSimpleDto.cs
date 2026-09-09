using System.ComponentModel.DataAnnotations;
using Lms_backend.Domain.Enums;

namespace Lms_backend.Application.Models;

public class ResourceSimpleDto
{
    public Guid Id { get; set; }
    public UserSimpleDto CreatedBy { get; set; } = default!;
    public string Name { get; set; } = string.Empty;
    public ResourceType Type { get; set; }
}
