namespace AdsPortal.WebApi.Domain.Entities
{
    using System;
    using AdsPortal.WebApi.Domain.Abstractions.Audit;
    using AdsPortal.WebApi.Domain.Abstractions.Base;
    using AdsPortal.WebApi.Domain.Enums;

    [AuditIgnore]
    public class Job : IBaseRelationalEntity, IEntityCreation
    {
        public Guid Id { get; set; }
        public ulong JobNo { get; set; }

        public DateTime CreatedOn { get; set; }
        public Guid? CreatedBy { get; set; }

        public DateTime? StartedOn { get; set; }
        public DateTime? FinishedOn { get; set; }
        public JobStatuses Status { get; set; }

        public Guid? Instance { get; set; }

        public string Operation { get; set; } = string.Empty;
        public int Priority { get; set; }
        public DateTime? PostponeTo { get; set; }
        public TimeSpan? TimeoutAfter { get; set; }

        public int Tries { get; set; }

        public string? Arguments { get; set; }
        public string? Exception { get; set; }
    }
}
