namespace AdsPortal.Persistence.Repository.Generic
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading;
    using System.Threading.Tasks;
    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using AdsPortal.Application.Exceptions;
    using AdsPortal.Application.Interfaces.Persistence.Repository.Generic;
    using AdsPortal.Domain.Abstractions.Base;
    using AdsPortal.Persistence.Interfaces.DbContext.Generic;
    using MongoDB.Driver;
    using MongoDB.Driver.Linq;

    public class GenericMongoReadOnlyRepository<TEntity> : IGenericMongoReadOnlyRepository<TEntity>
        where TEntity : class, IBaseMongoEntity
    {
        protected IGenericMongoDatabaseContext DbContext { get; }
        protected IMongoCollection<TEntity> DbSet { get; }
        protected IMapper Mapper { get; }

        public Type EntityType { get; }
        public string EntityName { get; }

        public GenericMongoReadOnlyRepository(IGenericMongoDatabaseContext context, IMapper mapper)
        {
            DbContext = context;
            DbSet = context.GetCollection<TEntity>();
            Mapper = mapper;

            Type type = typeof(TEntity);
            EntityType = type;
            EntityName = type.Name;
        }

        protected IMongoQueryable<TEntity> GetQueryable(Expression<Func<TEntity, bool>>? filter = null,
                                                        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null)
        {
            IMongoQueryable<TEntity> query = DbSet.AsQueryable();

            if (filter != null)
                query = query.Where(filter);

            if (orderBy != null)
                query = (IMongoQueryable<TEntity>)orderBy(query);

            return query;
        }

        #region IGenericMongoReadOnlyRepository<TEntity>
        public async Task<List<TEntity>> AllAsync(Expression<Func<TEntity, bool>>? filter = null,
                                                  Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null)
        {
            return await GetQueryable(filter, orderBy).ToListAsync();
        }

        public async Task<IEnumerable<TEntity>> GetAsync(Expression<Func<TEntity, bool>>? filter = null)
        {
            return await GetQueryable(filter).ToListAsync();
        }

        public async Task<TEntity> SingleAsync(Expression<Func<TEntity, bool>>? filter = null)
        {
            TEntity? entity = await GetQueryable(filter).SingleOrDefaultAsync();

            return entity ?? throw new NotFoundException(EntityName);
        }

        public async Task<TEntity?> SingleOrDefaultAsync(Expression<Func<TEntity, bool>>? filter = null)
        {
            return await GetQueryable(filter).SingleOrDefaultAsync();
        }

        public async Task<TEntity> FirstAsync(Expression<Func<TEntity, bool>>? filter = null,
                                               CancellationToken cancellationToken = default)
        {
            TEntity? entity;
            if (filter is null)
                entity = await DbSet.AsQueryable().FirstOrDefaultAsync(cancellationToken);
            else
                entity = await DbSet.AsQueryable().FirstOrDefaultAsync(filter, cancellationToken);

            return entity ?? throw new NotFoundException(EntityName);
        }

        public async Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>>? filter = null,
                                                        CancellationToken cancellationToken = default)
        {
            if (filter is null)
                return await DbSet.AsQueryable().FirstOrDefaultAsync(cancellationToken);

            return await DbSet.AsQueryable().FirstOrDefaultAsync(filter, cancellationToken);
        }

        public async Task<TEntity?> SingleByIdAsync(Guid id)
        {
            return await DbSet.AsQueryable().SingleOrDefaultAsync(x => x.Id == id);
        }

        public async Task<int> GetCountAsync(Expression<Func<TEntity, bool>>? filter = null)
        {
            return await GetQueryable(filter).CountAsync();
        }

        public async Task<bool> ExistsAsync(Expression<Func<TEntity, bool>>? filter = null)
        {
            return await GetQueryable(filter).AnyAsync();
        }

        public async Task<List<T>> ProjectToAsync<T>(Expression<Func<TEntity, bool>>? filter = null)
        {
            return await Task.Run(() => GetQueryable(filter).ProjectTo<T>(Mapper.ConfigurationProvider)
                                                            .ToList());
        }
        #endregion

        #region IGenericMongoReadOnlyRepository
        public Type GetEntityType()
        {
            return typeof(TEntity);
        }

        public async Task<List<IBaseMongoEntity>> AllAsync()
        {
            return (List<IBaseMongoEntity>)(IList<TEntity>)await DbSet.AsQueryable().ToListAsync();
        }

        async Task<IBaseMongoEntity> IGenericMongoReadOnlyRepository.SingleByIdAsync(Guid id)
        {
            TEntity? entity = await DbSet.AsQueryable().SingleOrDefaultAsync(x => x.Id == id);

            return entity ?? throw new NotFoundException(EntityName, id);
        }

        async Task<IBaseMongoEntity?> IGenericMongoReadOnlyRepository.SingleByIdOrDefaultAsync(Guid id)
        {
            return await DbSet.AsQueryable().SingleOrDefaultAsync(x => x.Id == id);
        }

        public async Task<int> GetCountAsync()
        {
            return await DbSet.AsQueryable().CountAsync();
        }
        #endregion
    }
}
