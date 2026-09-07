using Lms_backend.Application.Exceptions;
using Lms_backend.Application.Models;
using Lms_backend.Domain.Enums;

namespace Lms_backend.Application.Validators;

public static class ResourceValidator
{
    public static void ValidateChangeDto(ResourceForChangeDto dto)
    {
        if (dto.Name.Length < 3 || dto.Name.Length > 50) throw new ValidationException("Resource name has invalid length, must be between 3 and 50 character");
        if (dto.Description.Length < 3 || dto.Description.Length > 200) throw new ValidationException("Resource description has invalid length, must be between 3 and 200 character");
        if (!ResourceType.IsDefined(dto.Type)) throw new ValidationException($"Unknown resource type: {dto.Type}");
        if (dto.Data.Length > 2000) throw new ValidationException("Resource data is too large, can't exceed 2000 characters");
    }
}
