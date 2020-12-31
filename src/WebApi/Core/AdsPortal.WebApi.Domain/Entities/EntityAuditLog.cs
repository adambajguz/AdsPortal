namespace AdsPortal.WebApi.Domain.Entities
{
    using System;
    using AdsPortal.WebApi.Domain.Abstractions.Audit;
    using AdsPortal.WebApi.Domain.Abstractions.Base;
    using AdsPortal.WebApi.Domain.Abstractions.Enums;

    [AuditIgnore]
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
