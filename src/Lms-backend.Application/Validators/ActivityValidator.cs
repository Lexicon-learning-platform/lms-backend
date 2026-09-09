using Lms_backend.Application.Exceptions;
using Lms_backend.Application.Models;
using Lms_backend.Domain.Enums;

namespace Lms_backend.Application.Validators;

public static class ActivityValidator
{
    public static void ValidateChangeDto(ActivityForChangeDto dto)
    {
        if (dto.Name.Length < 3 || dto.Name.Length > 50) throw new ValidationException("Activity name has invalid length, must be between 3 and 50 character");
        if (dto.Description.Length < 3 || dto.Description.Length > 200) throw new ValidationException("Activity description has invalid length, must be between 3 and 200 character");
        if (dto.StartOffset < 0) throw new ValidationException("Activity start offset can't be negative");
        if (dto.Duration < 0) throw new ValidationException("Activity duration can't be negative");
        if (!ActivityType.IsDefined(dto.Type)) throw new ValidationException($"Unknown activity type: {dto.Type}");
    }
}
