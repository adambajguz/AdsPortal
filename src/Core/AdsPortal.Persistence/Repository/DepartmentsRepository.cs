﻿namespace AdsPortal.Persistence.Repository
{
    using AdsPortal.Application.Interfaces.Identity;
    using AdsPortal.Application.Interfaces.Persistence.Repository;
    using AdsPortal.Domain.Entities;
    using AdsPortal.Persistence.Interfaces.DbContext;
    using AdsPortal.Persistence.Repository.Generic;
    using AutoMapper;

    public class DepartmentsRepository : GenericRelationalRepository<Department>, IDepartmentsRepository
    {
        public DepartmentsRepository(ICurrentUserService currentUserService,
                                     IRelationalDbContext context,
                                     IMapper mapper) : base(currentUserService, context, mapper)
        {

        }
    }
}
