namespace AdsPortal.WebApi.Persistence.Extensions
{
    using System;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata;

    public static class EntityFrameworkExtensions
    {
        public static string GetTableName(this IModel model, Type entityType)
        {
            IEntityType? efEntityType = model.FindEntityType(entityType);

            return efEntityType?.GetTableName() ?? throw new NullReferenceException($"EntityFramework migrations possibly not applied. Entity of type {entityType.Name} does not have its representation in database");
        }
    }
}
