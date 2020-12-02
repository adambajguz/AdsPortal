namespace AdsPortal.Persistence.Repository
{
    using AutoMapper;
    using AdsPortal.Application.Interfaces.Identity;
    using AdsPortal.Application.Interfaces.Persistence.Repository;
    using AdsPortal.Domain.Entities;
    using AdsPortal.Persistence.Interfaces.DbContext;
    using AdsPortal.Persistence.Repository.Generic;

    public class DegreesRepository : GenericRelationalRepository<Degree>, IDegreesRepository
    {
        public DegreesRepository(ICurrentUserService currentUserService,
                                 IRelationalDbContext context,
                                 IMapper mapper) : base(currentUserService, context, mapper)
        {

        }
    }
}
