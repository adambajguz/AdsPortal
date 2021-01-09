namespace AdsPortal.WebApi.Application.Interfaces.Persistence.UoW.Generic
{
    using System.Threading;
    using System.Threading.Tasks;
    using AdsPortal.WebApi.Application.Interfaces.Persistence.Repository;

    public interface IGenericAuditableRelationalUnitOfWork : IGenericRelationalUnitOfWork
    {
        //Audit
        IEntityAuditLogsRepository EntityAuditLogs { get; }

        int SaveChangesWithoutAudit();
        Task<int> SaveChangesWithoutAuditAsync(CancellationToken cancellationToken = default);
    }
}
