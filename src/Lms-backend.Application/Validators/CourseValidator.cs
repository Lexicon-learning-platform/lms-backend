using Lms_backend.Application.Exceptions;
using Lms_backend.Application.Models;

namespace Lms_backend.Application.Validators;

public static class CourseValidator
{
    public static void ValidateChangeDto(CourseForChangeDto dto, bool isNew)
    {
        if (dto.Name.Length < 3 || dto.Name.Length > 50) throw new ValidationException("Course name has invalid length, must be between 3 and 50 character");
        if (dto.Description.Length < 3 || dto.Description.Length > 200) throw new ValidationException("Course description has invalid length, must be between 3 and 200 character");
        if (dto.Duration < 0) throw new ValidationException("Course duration can't be negative");

        var today = DateOnly.FromDateTime(DateTime.UtcNow);
        if (isNew)
        {
            if (dto.StartDate <= today) throw new ValidationException("Course start date must be in the future");
        }
        else
        {
            if (dto.StartDate < today.AddYears(-10)) throw new ValidationException("Course start date can't be more than 10 years in the past");
        }
    }
}
