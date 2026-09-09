using Lms_backend.Application.Exceptions;
using Lms_backend.Application.Models;
using Lms_backend.Domain.Enums;

namespace Lms_backend.Application.Validators;

public static class ModuleValidator
{
    public static void ValidateChangeDto(ModuleForChangeDto dto)
    {
        if (dto.Name.Length < 3 || dto.Name.Length > 50) throw new ValidationException("Module name has invalid length, must be between 3 and 50 character");
        if (dto.Description.Length < 3 || dto.Description.Length > 200) throw new ValidationException("Module description has invalid length, must be between 3 and 200 character");
        if (dto.Duration < 0) throw new ValidationException("Module duration can't be negative");
    }
}
