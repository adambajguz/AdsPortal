namespace AdsPortal.Persistence.Interfaces.DbContext
{
    using AdsPortal.Persistence.Interfaces.DbContext.Generic;
    using Domain.Entities;
    using MongoDB.Driver;

    public interface IMongoDbContext : IGenericMongoDatabaseContext
    {
        IMongoCollection<AnalyticsRecord> AnalyticsRecords { get; }
    }
}
