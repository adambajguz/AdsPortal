namespace AdsPortal.Persistence.Repository
{
    using AdsPortal.Persistence.Interfaces.DbContext;
    using AdsPortal.Persistence.Repository.Generic;
    using AdsPortal.WebApi.Application.Interfaces.Identity;
    using AdsPortal.WebApi.Application.Interfaces.Persistence.Repository;
    using AdsPortal.WebApi.Domain.Entities;
    using AutoMapper;

    public class EntityAuditLogsRepository : GenericRelationalRepository<EntityAuditLog>, IEntityAuditLogsRepository
    {
        public EntityAuditLogsRepository(ICurrentUserService currentUserService,
                                         IRelationalDbContext context,
                                         IMapper mapper) : base(currentUserService, context, mapper)
        {

        }
    }
}
