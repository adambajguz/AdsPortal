namespace AdsPortal.WebApi.Persistence.Repository.Generic
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using AdsPortal.Shared.Extensions.Extensions;
    using AdsPortal.WebApi.Application.Interfaces.Persistence;
    using AdsPortal.WebApi.Domain.Abstractions.Base;
    using AdsPortal.WebApi.Domain.Interfaces.Repository.Generic;
    using AdsPortal.WebApi.Persistence.Extensions;
    using AdsPortal.WebApi.Persistence.Interfaces.DbContext.Generic;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;

    public class RepositoryFactory : IRepositoryFactory
    {
        private static Dictionary<string, Type>? TableNameToEntityLookup { get; set; }

        private readonly IGenericRelationalDbContext _context;
        private readonly IServiceProvider _serviceProvider;

        public RepositoryFactory(IGenericRelationalDbContext context, IServiceProvider serviceProvider)
        {
            _context = context;
            _serviceProvider = serviceProvider;

            TableNameToEntityLookup ??= BuildTableNameToEntityLookup(context.Provider, "*.Domain*");
        }

        private static Dictionary<string, Type> BuildTableNameToEntityLookup(DbContext dbContext, params string[] assemblyFilters)
        {
            Type[] exportedTypes = AssemblyExtensions.GetAllExportedTypes(assemblyFilters);

            IEnumerable<Type> types = exportedTypes.Where(type => !type.IsAbstract &&
                                                                  type.Namespace != null &&
                                                                  !type.IsGenericTypeDefinition &&
                                                                  typeof(IBaseRelationalEntity).IsAssignableFrom(type));

            Dictionary<string, Type> tableNameToEntityLookup = new Dictionary<string, Type>();
            foreach (Type type in types)
            {
                string tableName = dbContext.Model.GetTableName(type);

                tableNameToEntityLookup.Add(tableName, type);
            }

            return tableNameToEntityLookup;
        }

        public Type GetEntityTypeFromTableName(string tableName)
        {
            if (!TableNameToEntityLookup!.TryGetValue(tableName, out Type? type))
            {
                throw new ArgumentException($"Invalid table name '{tableName}'", nameof(tableName));
            }

            return type;
        }

        public string GetTableNameFromEntityType<TEntity>()
            where TEntity : IBaseRelationalEntity
        {
            return _context.Provider.Model.GetTableName(typeof(TEntity));
        }

        public string GetTableNameFromEntityType(Type entityType)
        {
            return _context.Provider.Model.GetTableName(entityType);
        }

        public Type GetReadOnlyRepositoryType(string tableName)
        {
            Type type = GetEntityTypeFromTableName(tableName);
            return typeof(IGenericRelationalReadOnlyRepository<>).MakeGenericType(type);
        }

        public Type GetReadOnlyRepositoryType<TEntity>()
            where TEntity : class, IBaseRelationalEntity
        {
            return typeof(IGenericRelationalReadOnlyRepository<TEntity>);
        }

        public Type GetRepositoryType(string tableName)
        {
            Type type = GetEntityTypeFromTableName(tableName);
            return typeof(IGenericRelationalRepository<>).MakeGenericType(type);
        }

        public Type GetRepositoryType<TEntity>()
            where TEntity : class, IBaseRelationalEntity
        {
            return typeof(IGenericRelationalRepository<TEntity>);
        }

        public IGenericRelationalReadOnlyRepository CreateReadOnlyRepository(string tableName)
        {
            Type type = GetEntityTypeFromTableName(tableName);
            Type repositoryType = typeof(IGenericRelationalReadOnlyRepository<>).MakeGenericType(type);

            return (IGenericRelationalReadOnlyRepository)_serviceProvider.GetRequiredService(repositoryType);
        }

        public IGenericRelationalReadOnlyRepository<TEntity> CreateReadOnlyRepository<TEntity>()
            where TEntity : class, IBaseRelationalEntity
        {
            return _serviceProvider.GetRequiredService<IGenericRelationalReadOnlyRepository<TEntity>>();
        }

        public IGenericRelationalRepository CreateRepository(string tableName)
        {
            Type type = GetEntityTypeFromTableName(tableName);
            Type repositoryType = typeof(IGenericRelationalRepository<>).MakeGenericType(type);

            return (IGenericRelationalRepository)_serviceProvider.GetRequiredService(repositoryType);
        }

        public IGenericRelationalRepository<TEntity> CreateRepository<TEntity>()
            where TEntity : class, IBaseRelationalEntity
        {
            return _serviceProvider.GetRequiredService<IGenericRelationalRepository<TEntity>>();
        }

        public TSpecificRepositoryInterface CreateSpecificRepository<TSpecificRepositoryInterface>()
            where TSpecificRepositoryInterface : IGenericRelationalReadOnlyRepository
        {
            return _serviceProvider.GetRequiredService<TSpecificRepositoryInterface>();

        }
    }
}
