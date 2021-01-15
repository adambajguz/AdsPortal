namespace AdsPortal.WebApi.Domain.Interfaces.Repository
{
    using AdsPortal.WebApi.Domain.Entities;
    using AdsPortal.WebApi.Domain.Interfaces.Repository.Generic;

    public interface IEntityAuditLogsRepository : IGenericRelationalRepository<EntityAuditLog>
    {

    }
}
