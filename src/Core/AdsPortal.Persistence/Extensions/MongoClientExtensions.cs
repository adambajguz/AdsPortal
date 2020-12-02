namespace AdsPortal.Persistence.Extensions
{
    using AdsPortal.Domain.Abstractions.Base;
    using Humanizer;
    using MongoDB.Driver;

    public static class MongoClientExtensions
    {
        public static IMongoCollection<T> GetCollection<T>(this IMongoDatabase mongoDatabase, MongoCollectionSettings? mongoCollectionSettings = null)
           where T : class, IBaseMongoEntity
        {
            string collectionName = typeof(T).Name.Pluralize();

            return mongoDatabase.GetCollection<T>(collectionName, mongoCollectionSettings);
        }
    }
}
