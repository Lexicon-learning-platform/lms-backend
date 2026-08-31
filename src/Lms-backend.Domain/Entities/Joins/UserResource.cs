using Lms_backend.Domain.Entities;
﻿namespace Lms_backend.Domain.Entities.Joins
{
    public record UserResource
    {
        public Guid Id { get; set; }

        public int UserId { get; set; }

        public int ResourceId { get; set; }

        public ApplicationUser? User { get; set; }

        public Resource? Resource { get; set; }
    }
}
