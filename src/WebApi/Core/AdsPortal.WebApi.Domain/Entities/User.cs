namespace AdsPortal.WebApi.Domain.Entities
{
    using System;
    using System.Collections.Generic;
    using AdsPortal.WebApi.Domain.Abstractions.Audit;
    using AdsPortal.WebApi.Domain.Abstractions.Base;
    using AdsPortal.WebApi.Domain.Jwt;

    public class User : IBaseRelationalEntity, IEntityInfo, IJwtUserData
    {
        public Guid Id { get; set; }

        public DateTime CreatedOn { get; set; }
        public Guid? CreatedBy { get; set; }
        public DateTime LastSavedOn { get; set; }
        public Guid? LastSavedBy { get; set; }

        public string Email { get; set; } = string.Empty;

        [AuditIgnore]
        public string Password { get; set; } = string.Empty;

        public string Name { get; set; } = string.Empty;
        public string Surname { get; set; } = string.Empty;

        public Roles Role { get; set; }
        public bool IsActive { get; set; }

        public virtual ICollection<Advertisement> Advertisements { get; set; } = new HashSet<Advertisement>();
    }
}
