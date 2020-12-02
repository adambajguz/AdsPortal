namespace AdsPortal.Persistence.Interfaces.DbContext.Generic
{
    using AdsPortal.Domain.Abstractions.Base;
    using MongoDB.Driver;

    public interface IGenericMongoDatabaseContext
    {
        MongoClient DbClient { get; }
        IMongoDatabase Db { get; }

        public IMongoCollection<T> GetCollection<T>()
            where T : class, IBaseMongoEntity;
    }
}
