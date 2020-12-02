namespace AdsPortal.Domain.Entities
{
    using System;
    using AdsPortal.Domain.Abstractions.Audit;
    using AdsPortal.Domain.Abstractions.Base;
    using AdsPortal.Domain.Jwt;

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
        public string PhoneNumber { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;

        public Roles Role { get; set; }
        public bool IsActive { get; set; }
    }
}
