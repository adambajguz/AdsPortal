namespace AdsPortal.Persistence.Repository
{
    using AutoMapper;
    using AdsPortal.Application.Interfaces.Identity;
    using AdsPortal.Application.Interfaces.Persistence.Repository;
    using AdsPortal.Domain.Entities;
    using AdsPortal.Persistence.Interfaces.DbContext;
    using AdsPortal.Persistence.Repository.Generic;

    public class JobsRepository : GenericRelationalRepository<Job>, IJobsRepository
    {
        public JobsRepository(ICurrentUserService currentUserService,
                               IRelationalDbContext context,
                               IMapper mapper) : base(currentUserService, context, mapper)
        {

        }
    }
}
