using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Lms_backend.Domain.Entities.Joins
{
    public record CourseModule
    {
        [Key]
        public Guid Id { get; set; }

        public Guid CourseId { get; set; }

        public Guid ModuleId { get; set; }

        [ForeignKey("CourseId")]
        public Course Course { get; set; } = default!;

        [ForeignKey("ModuleId")]
        public Module Module { get; set; } = default!;

        public int StartTimeOffset { get; set; }
    }
}
