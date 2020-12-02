namespace AdsPortal.Application.Interfaces.Persistence.UoW.Generic
{
    using System.Threading;
    using System.Threading.Tasks;
    using AdsPortal.Application.Interfaces.Persistence.Repository;

    public interface IGenericAuditableRelationalUnitOfWork : IGenericRelationalUnitOfWork
    {
        IEntityAuditLogsRepository EntityAuditLogs { get; }

        int SaveChangesWithoutAudit();
        Task<int> SaveChangesWithoutAuditAsync(CancellationToken cancellationToken = default);
    }
}
