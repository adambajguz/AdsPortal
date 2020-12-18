namespace AdsPortal.Persistence.DbContext
{
    using System;
    using System.Linq.Expressions;
    using System.Threading.Tasks;
    using AdsPortal.Domain.Abstractions.Base;
    using AdsPortal.Domain.Entities;
    using AdsPortal.Persistence.Configurations;
    using AdsPortal.Persistence.Extensions;
    using AdsPortal.Persistence.Interfaces.DbContext;
    using Microsoft.Extensions.Options;
    using MongoDB.Driver;

    public class MongoDbContext : IMongoDbContext
    {
        public MongoClient DbClient { get; }
        public IMongoDatabase Db { get; }

        public MongoDbContext(IOptions<MongoDbConfiguration> options)
        {
            MongoDbConfiguration mongoConfiguration = options.Value;

            string? connectionString = mongoConfiguration.ConnectionString;
            if (string.IsNullOrWhiteSpace(connectionString))
                throw new ArgumentNullException(nameof(options));

            string? databaseName = mongoConfiguration.DatabaseName;
            if (string.IsNullOrWhiteSpace(databaseName))
                throw new ArgumentNullException(nameof(options));

            DbClient = new MongoClient(connectionString);
            Db = DbClient.GetDatabase(databaseName);

            AddOrUpdateAsync<AnalyticsRecord>(x => x.Hash).Wait();
            AddOrUpdateAsync<AnalyticsRecord>(x => x.Visits).Wait();
        }

        public IMongoCollection<AnalyticsRecord> AnalyticsRecords => Db.GetCollection<AnalyticsRecord>();

        public IMongoCollection<T> GetCollection<T>()
            where T : class, IBaseMongoEntity
        {
            return Db.GetCollection<T>();
        }

        public async Task AddOrUpdateAsync<TDocument>(Expression<Func<TDocument, object>> field)
            where TDocument : class, IBaseMongoEntity
        {
            IMongoCollection<TDocument> mongoCollection = GetCollection<TDocument>();
            //IAsyncCursor<BsonDocument> indexes = await mongoCollection.Indexes.ListAsync();

            CreateIndexModel<TDocument> indexModel = new CreateIndexModel<TDocument>(Builders<TDocument>.IndexKeys.Ascending(field));
            await mongoCollection.Indexes.CreateOneAsync(indexModel);
        }
    }
}
