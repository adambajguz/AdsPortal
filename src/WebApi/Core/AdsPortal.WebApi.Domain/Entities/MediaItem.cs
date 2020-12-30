namespace AdsPortal.Domain.Entities
{
    using System;
    using AdsPortal.Domain.Abstractions.Audit;
    using AdsPortal.Domain.Abstractions.Base;
    using AdsPortal.Domain.Jwt;

    public class MediaItem : IBaseRelationalEntity, IEntityInfo
    {
        public Guid Id { get; set; }

        public DateTime CreatedOn { get; set; }
        public Guid? CreatedBy { get; set; }
        public DateTime LastSavedOn { get; set; }
        public Guid? LastSavedBy { get; set; }

        public string FileName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Alt { get; set; } = string.Empty;

        public string VirtualDirectory { get; set; } = string.Empty;
        public long PathHashCode { get; set; }

        [AuditIgnore]
        public virtual byte[]? Data { get; set; }

        [AuditIgnore]
        public string ContentType { get; set; } = string.Empty;

        [AuditIgnore]
        public string Hash { get; set; } = string.Empty;

        [AuditIgnore]
        public long ByteSize { get; set; }

        public Guid? OwnerId { get; set; }
        public Roles Role { get; set; }
    }
}
