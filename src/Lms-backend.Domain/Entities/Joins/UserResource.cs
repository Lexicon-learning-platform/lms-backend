using Lms_backend.Domain.Entities;
﻿namespace Lms_backend.Domain.Entities.Joins
{
    public record UserResource
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }

        public Guid ResourceId { get; set; }

        public ApplicationUser? User { get; set; }

        public Resource? Resource { get; set; }
    }
}
