﻿namespace AdsPortal.WebApi.Persistence.Repository
{
    using AdsPortal.WebApi.Application.Interfaces.Identity;
    using AdsPortal.WebApi.Domain.Entities;
    using AdsPortal.WebApi.Domain.Interfaces.Repository;
    using AdsPortal.WebApi.Persistence.Interfaces.DbContext;
    using AdsPortal.WebApi.Persistence.Repository.Generic;
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
