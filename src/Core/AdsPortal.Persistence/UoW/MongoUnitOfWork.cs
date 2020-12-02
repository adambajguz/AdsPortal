namespace AdsPortal.Persistence.UoW
{
    using System;
    using AutoMapper;
    using AdsPortal.Application.Interfaces.Identity;
    using AdsPortal.Application.Interfaces.Persistence.Repository;
    using AdsPortal.Application.Interfaces.Persistence.UoW;
    using AdsPortal.Persistence.Interfaces.DbContext;
    using AdsPortal.Persistence.Repository;
    using AdsPortal.Persistence.UoW.Generic;

    public class MongoUnitOfWork : GenericMongoUnitOfWork, IMongoUnitOfWork
    {
        private readonly Lazy<IAnalyticsRecordsRepository> _analyticsRecords;
        public IAnalyticsRecordsRepository AnalyticsRecords => _analyticsRecords.Value;

        public MongoUnitOfWork(ICurrentUserService currentUserService, IMongoDbContext context, IMapper mapper) : base(currentUserService, context, mapper)
        {
            _analyticsRecords = new Lazy<IAnalyticsRecordsRepository>(() => GetSpecificRepository<IAnalyticsRecordsRepository, AnalyticsRecordsRepository>());
        }
    }
}
