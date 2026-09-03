using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Lms_backend.Domain.Interfaces;

namespace Lms_backend.Domain.Entities.Joins
{
    public record CourseResource : IEntity
    {
        [Key]
        public Guid Id { get; set; }

        public Guid CourseId { get; set; }

        public Guid ResourceId { get; set; }

        [ForeignKey("CourseId")]
        public Course Course { get; set; } = default!;

        [ForeignKey("ResourceId")]
        public Resource Resource { get; set; } = default!;
    }
}
