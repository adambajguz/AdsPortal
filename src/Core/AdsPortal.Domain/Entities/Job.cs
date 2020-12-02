namespace AdsPortal.Domain.Entities
{
    using System;
    using AdsPortal.Domain.Abstractions.Audit;
    using AdsPortal.Domain.Abstractions.Base;
    using AdsPortal.Domain.Enums;

    [AuditIgnore]
    public class Job : IBaseRelationalEntity, IEntityCreation
    {
        public Guid Id { get; set; }
        public long JobNo { get; set; }

        public DateTime CreatedOn { get; set; }
        public Guid? CreatedBy { get; set; }

        public DateTime? StartedOn { get; set; }
        public DateTime? FinishedOn { get; set; }
        public JobStatuses Status { get; set; }

        public string Operation { get; set; } = string.Empty;
        public ushort Priority { get; set; }
        public DateTime? PostponeTo { get; set; }

        public string? OperationArguments { get; set; }
        public string? OperationResult { get; set; }
    }
}
