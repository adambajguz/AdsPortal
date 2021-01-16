namespace AdsPortal.WebApi.Persistence.UoW.Generic
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using AdsPortal.WebApi.Application.Interfaces.Identity;
    using AdsPortal.WebApi.Application.Interfaces.Persistence;
    using AdsPortal.WebApi.Domain.Abstractions.Base;
    using AdsPortal.WebApi.Domain.Interfaces.Repository.Generic;
    using AdsPortal.WebApi.Domain.Interfaces.UoW.Generic;
    using AdsPortal.WebApi.Persistence.Interfaces.DbContext.Generic;
    using AutoMapper;
    using Microsoft.EntityFrameworkCore;

    public abstract class GenericRelationalUnitOfWork : IGenericRelationalUnitOfWork, IDisposable
    {
        protected IRepositoryFactory RepositoryFactory { get; }
        protected ICurrentUserService CurrentUser { get; }
        protected IGenericRelationalDbContext Context { get; }
        protected DbContext Provider { get; private set; }
        protected IMapper Mapper { get; private set; }

        public bool IsDisposed { get; private set; }

        protected ConcurrentDictionary<Type, IGenericRelationalReadOnlyRepository> Repositories { get; } = new();

        protected GenericRelationalUnitOfWork(IRepositoryFactory repositoryFactory, ICurrentUserService currentUserService, IGenericRelationalDbContext context, IMapper mapper)
        {
            RepositoryFactory = repositoryFactory;
            CurrentUser = currentUserService;
            Context = context;
            Provider = context.Provider;
            Mapper = mapper;
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

        public IGenericRelationalReadOnlyRepository GetReadOnlyRepositoryByName(string tableName)
        {
            Type repositoryType = RepositoryFactory.GetReadOnlyRepositoryType(tableName);

            if (!Repositories.TryGetValue(repositoryType, out IGenericRelationalReadOnlyRepository? repository))
            {
                repository = RepositoryFactory.CreateReadOnlyRepository(tableName);
                Repositories.TryAdd(repositoryType, repository);
            }

            return repository;
        }

        public IGenericRelationalReadOnlyRepository<TEntity> GetReadOnlyRepository<TEntity>()
            where TEntity : class, IBaseRelationalEntity
        {
            Type repositoryType = RepositoryFactory.GetReadOnlyRepositoryType<TEntity>();

            if (!Repositories.TryGetValue(repositoryType, out IGenericRelationalReadOnlyRepository? repository))
            {
                var newRepository = RepositoryFactory.CreateReadOnlyRepository<TEntity>();
                Repositories.TryAdd(repositoryType, newRepository);

                return newRepository;
            }

            return (IGenericRelationalReadOnlyRepository<TEntity>)repository;
        }

        public IGenericRelationalRepository GetRepositoryByName(string tableName)
        {
            Type repositoryType = RepositoryFactory.GetRepositoryType(tableName);

            if (!Repositories.TryGetValue(repositoryType, out IGenericRelationalReadOnlyRepository? repository))
            {
                var newRepository = RepositoryFactory.CreateRepository(tableName);
                Repositories.TryAdd(repositoryType, newRepository);

                return newRepository;
            }

            return (IGenericRelationalRepository)repository;
        }

        public IGenericRelationalRepository<TEntity> GetRepository<TEntity>()
            where TEntity : class, IBaseRelationalEntity
        {
            Type repositoryType = RepositoryFactory.GetRepositoryType<TEntity>();

            if (!Repositories.TryGetValue(repositoryType, out IGenericRelationalReadOnlyRepository? repository))
            {
                var newRepository = RepositoryFactory.CreateRepository<TEntity>();
                Repositories.TryAdd(repositoryType, newRepository);

                return newRepository;
            }

            return (IGenericRelationalRepository<TEntity>)repository;
        }

        public TSpecificRepositoryInterface GetSpecificRepository<TSpecificRepositoryInterface>()
            where TSpecificRepositoryInterface : IGenericRelationalReadOnlyRepository
        {
            Type repositoryType = typeof(TSpecificRepositoryInterface);

            if (!Repositories.TryGetValue(repositoryType, out IGenericRelationalReadOnlyRepository? repository))
            {
                var newRepository = RepositoryFactory.CreateSpecificRepository<TSpecificRepositoryInterface>();
                Repositories.TryAdd(repositoryType, newRepository);

                return newRepository;
            }

            return (TSpecificRepositoryInterface)repository;
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