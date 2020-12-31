namespace AdsPortal.Persistence.Repository
{
    using AdsPortal.Application.Interfaces.Identity;
    using AdsPortal.Application.Interfaces.Persistence.Repository;
    using AdsPortal.Persistence.Interfaces.DbContext;
    using AdsPortal.Persistence.Repository.Generic;
    using AdsPortal.WebApi.Domain.Entities;
    using AutoMapper;

    public class JobsRepository : GenericRelationalRepository<Job>, IJobsRepository
    {
        public JobsRepository(ICurrentUserService currentUserService,
                               IRelationalDbContext context,
                               IMapper mapper) : base(currentUserService, context, mapper)
        {

        }
    }
}
