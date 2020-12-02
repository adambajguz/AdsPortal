namespace AdsPortal.Persistence.UoW.Generic
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using AdsPortal.Application.Interfaces.Identity;
    using AdsPortal.Application.Interfaces.Persistence.Repository.Generic;
    using AdsPortal.Application.Interfaces.Persistence.UoW.Generic;
    using AdsPortal.Domain.Abstractions.Base;
    using AdsPortal.Persistence.Interfaces.DbContext.Generic;
    using AdsPortal.Persistence.Repository.Generic;
    using AutoMapper;

    public abstract class GenericMongoUnitOfWork : IGenericMongoUnitOfWork, IDisposable
    {
        protected ICurrentUserService CurrentUser { get; private set; }
        protected IGenericMongoDatabaseContext Context { get; private set; }
        protected IMapper Mapper { get; private set; }

        public bool IsDisposed { get; private set; }

        private Dictionary<Type, IGenericMongoReadOnlyRepository> Repositories { get; } = new Dictionary<Type, IGenericMongoReadOnlyRepository>();

        protected GenericMongoUnitOfWork(ICurrentUserService currentUserService, IGenericMongoDatabaseContext context, IMapper mapper)
        {
            CurrentUser = currentUserService;
            Context = context;
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
                //Context.Dispose();
                Repositories.Clear();

            IsDisposed = true;
        }

        public IGenericMongoRepository<TEntity> GetRepository<TEntity>()
            where TEntity : class, IBaseMongoEntity
        {
            Type type = typeof(IGenericMongoRepository<TEntity>);
            if (Repositories.TryGetValue(type, out IGenericMongoReadOnlyRepository? value))
                return (value as IGenericMongoRepository<TEntity>)!;

            IGenericMongoRepository<TEntity> repository = (Activator.CreateInstance(typeof(GenericMongoRepository<TEntity>), CurrentUser, Context, Mapper) as IGenericMongoRepository<TEntity>)!;
            Repositories.Add(type, repository);
            return repository;
        }

        public IGenericMongoReadOnlyRepository<TEntity> GetReadOnlyRepository<TEntity>()
            where TEntity : class, IBaseMongoEntity
        {
            Type type = typeof(IGenericMongoReadOnlyRepository<TEntity>);
            if (Repositories.TryGetValue(type, out IGenericMongoReadOnlyRepository? value))
                return (value as IGenericMongoReadOnlyRepository<TEntity>)!;

            IGenericMongoReadOnlyRepository<TEntity> repository = (Activator.CreateInstance(typeof(GenericMongoReadOnlyRepository<TEntity>), CurrentUser, Context, Mapper) as IGenericMongoReadOnlyRepository<TEntity>)!;
            Repositories.Add(type, repository);
            return repository;
        }

        protected TSpecificRepositoryInterface GetSpecificRepository<TSpecificRepositoryInterface, TSpecificRepository>()
            where TSpecificRepositoryInterface : IGenericMongoReadOnlyRepository
            where TSpecificRepository : class, IGenericMongoReadOnlyRepository
        {
            Type type = typeof(TSpecificRepositoryInterface);
            if (Repositories.ContainsKey(type))
                return (TSpecificRepositoryInterface)Repositories[type];

            TSpecificRepositoryInterface repository = (TSpecificRepositoryInterface)Activator.CreateInstance(typeof(TSpecificRepository), CurrentUser, Context, Mapper)!;
            Repositories.Add(type, repository);
            return repository;
        }

        public int SaveChanges()
        {
            throw new NotImplementedException();
            //using (IClientSessionHandle Session = Context.DbClient.StartSession())
            //{
            //    Session.StartTransaction();

            //    IEnumerable<Task> commandTasks = _commands.Select(c => c());
            //    Task.WhenAll(commandTasks).Wait();

            //    Session.CommitTransaction();
            //}

            //return _commands.Count;
        }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();

            //using (IClientSessionHandle Session = await Context.DbClient.StartSessionAsync())
            //{
            //    Session.StartTransaction();

            //    IEnumerable<Task> commandTasks = _commands.Select(c => c());
            //    await Task.WhenAll(commandTasks);

            //    await Session.CommitTransactionAsync(cancellationToken);
            //}

            //return _commands.Count;
        }
    }
}