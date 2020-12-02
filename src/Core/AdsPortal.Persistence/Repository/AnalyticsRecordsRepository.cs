namespace AdsPortal.Persistence.Repository
{
    using AdsPortal.Application.Interfaces.Identity;
    using AdsPortal.Application.Interfaces.Persistence.Repository;
    using AdsPortal.Domain.Entities;
    using AdsPortal.Persistence.Interfaces.DbContext.Generic;
    using AdsPortal.Persistence.Repository.Generic;
    using AutoMapper;

    public class AnalyticsRecordsRepository : GenericMongoRepository<AnalyticsRecord>, IAnalyticsRecordsRepository
    {
        public AnalyticsRecordsRepository(ICurrentUserService currentUserService,
                                          IGenericMongoDatabaseContext context,
                                          IMapper mapper) : base(currentUserService, context, mapper)
        {

        }
    }
}
