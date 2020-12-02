namespace AdsPortal.Application.Operations.EntityAuditLogOperations.Commands.CleanupEntityAuditLog
{
    using System;
    using AdsPortal.Application.OperationsAbstractions;

    public class CleanupEntityAuditLogRequest : IOperation
    {
        public DateTime CreatedOn { get; set; }
        public string? TableName { get; set; }
        public Guid? Key { get; set; }
    }
}
