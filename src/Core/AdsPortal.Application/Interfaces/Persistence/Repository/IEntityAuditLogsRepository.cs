﻿namespace AdsPortal.Application.Interfaces.Persistence.Repository
{
    using AdsPortal.Application.Interfaces.Persistence.Repository.Generic;
    using AdsPortal.Domain.Entities;

    public interface IEntityAuditLogsRepository : IGenericRelationalRepository<EntityAuditLog>
    {

    }
}
