namespace AdsPortal.Domain.Abstractions.Base
{
    using System;
    using AdsPortal.Domain.Abstractions.Audit;

    public interface IEntityLastSaved
    {
        [AuditIgnore]
        public DateTime LastSavedOn { get; set; }

        [AuditIgnore]
        public Guid? LastSavedBy { get; set; }
    }
}