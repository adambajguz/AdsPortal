namespace AdsPortal.WebApi.Domain.Interfaces.UoW.Generic
{
    using System.Threading;
    using System.Threading.Tasks;
    using AdsPortal.WebApi.Domain.Interfaces.Repository;

    public interface IGenericAuditableRelationalUnitOfWork : IGenericRelationalUnitOfWork
    {
        //Audit
        IEntityAuditLogsRepository EntityAuditLogs { get; }

        int SaveChangesWithoutAudit();
        Task<int> SaveChangesWithoutAuditAsync(CancellationToken cancellationToken = default);
    }
}
