namespace AdsPortal.Application.Operations.EntityAuditLogOperations.Commands.CreateRouteLog
{
    using System;
    using AdsPortal.Application.OperationsAbstractions;

    public class RevertUsingEntityAuditLogRequest : IOperation
    {
        public Guid EntityId { get; set; }
        public Guid ToId { get; set; }
    }
}
