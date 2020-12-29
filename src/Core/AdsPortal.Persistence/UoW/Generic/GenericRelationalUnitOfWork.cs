namespace AdsPortal.Persistence.UoW.Generic
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using AdsPortal.Application.Interfaces.Identity;
    using AdsPortal.Application.Interfaces.Persistence.Repository.Generic;
    using AdsPortal.Application.Interfaces.Persistence.UoW.Generic;
    using AdsPortal.Common.Extensions;
    using AdsPortal.Domain.Abstractions.Base;
    using AdsPortal.Infrastructure.Extensions;
    using AdsPortal.Persistence.Interfaces.DbContext.Generic;
    using AdsPortal.Persistence.Repository.Generic;
    using AutoMapper;
    using Microsoft.EntityFrameworkCore;

    public abstract class GenericRelationalUnitOfWork : IGenericRelationalUnitOfWork, IDisposable
    {
        protected ICurrentUserService CurrentUser { get; private set; }
        protected IGenericRelationalDbContext Context { get; private set; }
        protected DbContext Provider { get; private set; }
        protected IMapper Mapper { get; private set; }

        public bool IsDisposed { get; private set; }

        private Dictionary<Type, IGenericRelationalReadOnlyRepository> Repositories { get; } = new Dictionary<Type, IGenericRelationalReadOnlyRepository>();
        private Dictionary<string, Type> TableNameToEntityLookup { get; } = new Dictionary<string, Type>();

        protected GenericRelationalUnitOfWork(ICurrentUserService currentUserService, IGenericRelationalDbContext context, IMapper mapper)
        {
            CurrentUser = currentUserService;
            Context = context;
            Provider = context.Provider;// as DbContext ?? throw new InvalidCastException($"{typeof(IGenericRelationalDbContext).FullName} should be castable to {typeof(DbContext).FullName}");
            Mapper = mapper;

            TableNameToEntityLookup = BuildTableNameToEntityLookup(Provider, "*.Domain*");
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

        protected Type GetEntityTypeFromTableName(string tableName)
        {
            bool found = TableNameToEntityLookup.TryGetValue(tableName, out Type? type);
            if (!found)
                throw new ArgumentException($"Invalid table name '{tableName}'", nameof(tableName));

            if (type is null)
                throw new ArgumentNullException(nameof(type));

            return type;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!IsDisposed && disposing)
            {
                Provider.Dispose();
                Repositories.Clear();
            }

            IsDisposed = true;
        }

        public IGenericRelationalRepository GetRepositoryByName(string name)
        {
            Type type = GetEntityTypeFromTableName(name);
            if (Repositories.TryGetValue(type, out IGenericRelationalReadOnlyRepository? value))
                return (value as IGenericRelationalRepository)!;

            Type constructingType = typeof(GenericRelationalRepository<>).MakeGenericType(type);
            IGenericRelationalRepository repository = (Activator.CreateInstance(constructingType, CurrentUser, Context, Mapper) as IGenericRelationalRepository)!;
            Repositories.Add(type, repository);

            return repository;
        }

        public IGenericRelationalRepository<TEntity> GetRepository<TEntity>()
            where TEntity : class, IBaseRelationalEntity
        {
            Type type = typeof(IGenericRelationalRepository<TEntity>);
            if (Repositories.TryGetValue(type, out IGenericRelationalReadOnlyRepository? value))
                return (value as IGenericRelationalRepository<TEntity>)!;

            IGenericRelationalRepository<TEntity> repository = (Activator.CreateInstance(typeof(GenericRelationalRepository<TEntity>), CurrentUser, Context, Mapper) as IGenericRelationalRepository<TEntity>)!;
            Repositories.Add(type, repository);

            return repository;
        }

        public IGenericRelationalReadOnlyRepository<TEntity> GetReadOnlyRepository<TEntity>()
            where TEntity : class, IBaseRelationalEntity
        {
            Type type = typeof(IGenericRelationalReadOnlyRepository<TEntity>);
            if (Repositories.TryGetValue(type, out IGenericRelationalReadOnlyRepository? value))
                return (value as IGenericRelationalReadOnlyRepository<TEntity>)!;

            IGenericRelationalReadOnlyRepository<TEntity> repository = (Activator.CreateInstance(typeof(GenericReadOnlyRelationalRepository<TEntity>), CurrentUser, Context, Mapper) as IGenericRelationalReadOnlyRepository<TEntity>)!;
            Repositories.Add(type, repository);

            return repository;
        }

        public IGenericRelationalReadOnlyRepository GetReadOnlyRepositoryByName(string name)
        {
            Type type = GetEntityTypeFromTableName(name);
            if (Repositories.TryGetValue(type, out IGenericRelationalReadOnlyRepository? value))
                return value!;

            Type constructingType = typeof(GenericReadOnlyRelationalRepository<>).MakeGenericType(type);
            IGenericRelationalReadOnlyRepository repository = (Activator.CreateInstance(constructingType, CurrentUser, Context, Mapper) as IGenericRelationalReadOnlyRepository)!;
            Repositories.Add(type, repository);

            return repository;
        }

        protected TSpecificRepositoryInterface GetSpecificRepository<TSpecificRepositoryInterface, TSpecificRepository>()
            where TSpecificRepositoryInterface : IGenericRelationalReadOnlyRepository
            where TSpecificRepository : class, IGenericRelationalReadOnlyRepository
        {
            Type type = typeof(TSpecificRepositoryInterface);
            if (Repositories.ContainsKey(type))
                return (TSpecificRepositoryInterface)Repositories[type];

            TSpecificRepositoryInterface repository = (TSpecificRepositoryInterface)Activator.CreateInstance(typeof(TSpecificRepository), CurrentUser, Context, Mapper)!;
            Repositories.Add(type, repository);

            return repository;
        }

        public virtual int SaveChanges()
        {
            return Provider.SaveChanges();
        }

        public virtual async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await Provider.SaveChangesAsync(cancellationToken);
        }
    }
}