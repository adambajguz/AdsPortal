namespace AdsPortal.Domain.Entities
{
    using System;
    using AdsPortal.Domain.Abstractions.Audit;
    using AdsPortal.Domain.Abstractions.Base;
    using AdsPortal.Domain.Abstractions.Enums;

    public class EntityAuditLog : IBaseRelationalEntity, IEntityCreation, IAuditLog
    {
        public Guid Id { get; set; }
        public DateTime CreatedOn { get; set; }
        public Guid? CreatedBy { get; set; }

        public string TableName { get; set; } = string.Empty;
        public Guid Key { get; set; }
        public AuditActions Action { get; set; }
        public string? Values { get; set; }
    }
}
