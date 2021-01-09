namespace AdsPortal.WebApi.Application.Interfaces.Persistence.Repository
{
    using AdsPortal.WebApi.Application.Interfaces.Persistence.Repository.Generic;
    using AdsPortal.WebApi.Domain.Entities;

    public interface IEntityAuditLogsRepository : IGenericRelationalRepository<EntityAuditLog>
    {

    }
}
