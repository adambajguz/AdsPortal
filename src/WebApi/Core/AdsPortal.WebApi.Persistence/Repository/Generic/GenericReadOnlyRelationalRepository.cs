namespace AdsPortal.WebApi.Persistence.Repository.Generic
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading;
    using System.Threading.Tasks;
    using AdsPortal.WebApi.Application.Exceptions;
    using AdsPortal.WebApi.Domain.Abstractions.Base;
    using AdsPortal.WebApi.Domain.Interfaces.Repository.Generic;
    using AdsPortal.WebApi.Persistence.Interfaces.DbContext.Generic;
    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using Microsoft.EntityFrameworkCore;

    //TODO: use project to instead of map
    public class GenericReadOnlyRelationalRepository<TEntity> : IGenericRelationalReadOnlyRepository<TEntity>
        where TEntity : class, IBaseRelationalEntity
    {
        protected IGenericRelationalDbContext Context { get; }
        protected DbContext Provider { get; }
        protected DbSet<TEntity> DbSet { get; }
        protected IMapper Mapper { get; }

        public Type EntityType { get; }
        public string EntityName { get; }

        public GenericReadOnlyRelationalRepository(IGenericRelationalDbContext dbContext, IMapper mapper)
        {
            Context = dbContext;
            Provider = dbContext.Provider;
            DbSet = Provider.Set<TEntity>();
            Mapper = mapper;

            Type type = typeof(TEntity);
            EntityType = type;
            EntityName = type.Name;
        }

        #region GetQueryable
        protected IQueryable<TEntity> GetQueryable(Expression<Func<TEntity, bool>>? filter)
        {
            IQueryable<TEntity> query = DbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            return query;
        }

        protected IQueryable<TEntity> GetQueryable(Expression<Func<TEntity, bool>>? filter,
                                                   Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy,
                                                   bool noTracking = false,
                                                   int? skip = null,
                                                   int? take = null)
        {
            IQueryable<TEntity> query = DbSet;

            if (noTracking)
            {
                query = query.AsNoTracking();
            }

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (orderBy != null)
            {
                query = orderBy(query);
            }

            if (skip is int s)
            {
                query = query.Skip(s);
            }

            if (take is int t)
            {
                query = query.Take(t);
            }

            return query;
        }
        #endregion

        #region IGenericRelationalReadOnlyRepository<TEntity>
        public async Task<List<TEntity>> AllAsync(Expression<Func<TEntity, bool>>? filter = null,
                                                  Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
                                                  bool noTracking = false,
                                                  int? skip = null,
                                                  int? take = null,
                                                  CancellationToken cancellationToken = default)
        {
            return await GetQueryable(filter, orderBy, noTracking, skip, take).ToListAsync(cancellationToken);
        }

        public async Task<TEntity> SingleAsync(Expression<Func<TEntity, bool>>? filter = null,
                                               bool noTracking = false,
                                               CancellationToken cancellationToken = default)
        {
            IQueryable<TEntity> query = noTracking ? DbSet.AsNoTracking() : DbSet;

            if (filter is null)
            {
                return await query.SingleOrDefaultAsync(cancellationToken) ?? throw new NotFoundException(EntityName);
            }

            return await query.SingleOrDefaultAsync(filter, cancellationToken) ?? throw new NotFoundException(EntityName);
        }

        public async Task<TEntity?> SingleOrDefaultAsync(Expression<Func<TEntity, bool>>? filter = null,
                                                         bool noTracking = false,
                                                         CancellationToken cancellationToken = default)
        {
            IQueryable<TEntity> query = noTracking ? DbSet.AsNoTracking() : DbSet;

            if (filter is null)
            {
                return await query.SingleOrDefaultAsync(cancellationToken);
            }

            return await query.SingleOrDefaultAsync(filter, cancellationToken);
        }

        public async Task<TEntity> FirstAsync(Expression<Func<TEntity, bool>>? filter = null,
                                              bool noTracking = false,
                                              CancellationToken cancellationToken = default)
        {
            IQueryable<TEntity> query = noTracking ? DbSet.AsNoTracking() : DbSet;

            TEntity? entity;
            if (filter is null)
            {
                entity = await query.FirstOrDefaultAsync(cancellationToken);
            }
            else
            {
                entity = await query.FirstOrDefaultAsync(filter, cancellationToken);
            }

            return entity ?? throw new NotFoundException(EntityName);
        }

        public async Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>>? filter = null,
                                                        bool noTracking = false,
                                                        CancellationToken cancellationToken = default)
        {
            IQueryable<TEntity> query = noTracking ? DbSet.AsNoTracking() : DbSet;

            if (filter is null)
            {
                return await query.FirstOrDefaultAsync(cancellationToken);
            }

            return await query.FirstOrDefaultAsync(filter, cancellationToken);
        }

        public async Task<TEntity> SingleByIdAsync(Guid id, bool noTracking = false, CancellationToken cancellationToken = default)
        {
            IQueryable<TEntity> query = noTracking ? DbSet.AsNoTracking() : DbSet;

            TEntity? entity = await query.SingleOrDefaultAsync(x => x.Id == id, cancellationToken);

            return entity ?? throw new NotFoundException(EntityName, id);
        }

        public async Task<TEntity?> SingleByIdOrDefaultAsync(Guid id, bool noTracking = false, CancellationToken cancellationToken = default)
        {
            IQueryable<TEntity> query = noTracking ? DbSet.AsNoTracking() : DbSet;

            return await query.SingleOrDefaultAsync(x => x.Id == id, cancellationToken);
        }

        public async Task<TEntity> SingleByIdWithRelatedAsync<TProperty0>(Guid id,
                                                                          Expression<Func<TEntity, TProperty0>> relatedSelector0,
                                                                          bool noTracking = false,
                                                                          CancellationToken cancellationToken = default)
        {
            return await SingleByIdWithRelatedOrDefaultAsync(id, relatedSelector0, noTracking, cancellationToken) ?? throw new NotFoundException(EntityName);
        }

        public async Task<TEntity> SingleByIdWithRelatedAsync<TProperty0, TProperty1>(Guid id,
                                                                                      Expression<Func<TEntity, TProperty0>> relatedSelector0,
                                                                                      Expression<Func<TEntity, TProperty1>> relatedSelector1,
                                                                                      bool noTracking = false,
                                                                                      CancellationToken cancellationToken = default)
        {
            return await SingleByIdWithRelatedOrDefaultAsync(id, relatedSelector0, relatedSelector1, noTracking, cancellationToken) ?? throw new NotFoundException(EntityName);
        }

        public async Task<TEntity> SingleByIdWithRelatedAsync(Guid id,
                                                              bool noTracking = false,
                                                              CancellationToken cancellationToken = default,
                                                              params Expression<Func<TEntity, object>>[] relatedSelectors)
        {
            return await SingleByIdWithRelatedOrDefaultAsync(id, noTracking, cancellationToken, relatedSelectors) ?? throw new NotFoundException(EntityName);
        }

        public async Task<TEntity?> SingleByIdWithRelatedOrDefaultAsync<TProperty0>(Guid id,
                                                                                    Expression<Func<TEntity, TProperty0>> relatedSelector0,
                                                                                    bool noTracking = false,
                                                                                    CancellationToken cancellationToken = default)
        {
            IQueryable<TEntity> query = DbSet.Include(relatedSelector0);

            if (noTracking)
            {
                query = query.AsNoTracking();
            }

            return await query.SingleOrDefaultAsync(x => x.Id == id, cancellationToken);
        }

        public async Task<TEntity?> SingleByIdWithRelatedOrDefaultAsync<TProperty0, TProperty1>(Guid id,
                                                                                                Expression<Func<TEntity, TProperty0>> relatedSelector0,
                                                                                                Expression<Func<TEntity, TProperty1>> relatedSelector1,
                                                                                                bool noTracking = false,
                                                                                                CancellationToken cancellationToken = default)
        {
            IQueryable<TEntity> query = DbSet.Include(relatedSelector0)
                                             .Include(relatedSelector1);

            if (noTracking)
            {
                query = query.AsNoTracking();
            }

            return await query.SingleOrDefaultAsync(x => x.Id == id, cancellationToken);
        }

        public async Task<TEntity?> SingleByIdWithRelatedOrDefaultAsync(Guid id,
                                                                        bool noTracking = false,
                                                                        CancellationToken cancellationToken = default,
                                                                        params Expression<Func<TEntity, object>>[] relatedSelectors)
        {
            IQueryable<TEntity> query = DbSet.AsQueryable();

            foreach (Expression<Func<TEntity, object>> relatedExpr in relatedSelectors)
            {
                query = query.Include(relatedExpr);
            }

            return await query.SingleOrDefaultAsync(x => x.Id == id, cancellationToken);
        }

        public async Task<int> GetCountAsync(Expression<Func<TEntity, bool>>? filter = null, CancellationToken cancellationToken = default)
        {
            if (filter is null)
            {
                return await DbSet.CountAsync(cancellationToken);
            }

            return await DbSet.CountAsync(filter, cancellationToken);
        }

        public async Task<bool> ExistsAsync(Expression<Func<TEntity, bool>>? filter = null, CancellationToken cancellationToken = default)
        {
            if (filter is null)
            {
                return await DbSet.AnyAsync(cancellationToken);
            }

            return await DbSet.AnyAsync(filter, cancellationToken);
        }

        public async Task ExistsElseThrowAsync(Expression<Func<TEntity, bool>>? filter = null, CancellationToken cancellationToken = default)
        {
            bool exists;
            if (filter is null)
            {
                exists = await DbSet.AnyAsync(cancellationToken);
            }
            else
            {
                exists = await DbSet.AnyAsync(filter, cancellationToken);
            }

            if (!exists)
            {
                throw new NotFoundException(EntityName);
            }
        }
        #endregion

        #region IGenericRelationalReadOnlyRepository
        public async Task<List<IBaseRelationalEntity>> AllAsync(bool noTracking = false,
                                                                int? skip = null,
                                                                int? take = null,
                                                                CancellationToken cancellationToken = default)
        {
            return (List<IBaseRelationalEntity>)(IList<TEntity>)await GetQueryable(null, null, noTracking, skip, take).ToListAsync(cancellationToken);
        }

        async Task<IBaseRelationalEntity> IGenericRelationalReadOnlyRepository.SingleByIdAsync(Guid id, bool noTracking, CancellationToken cancellationToken)
        {
            IQueryable<TEntity> query = DbSet;

            if (noTracking)
            {
                query = query.AsNoTracking();
            }

            TEntity? entity = await query.SingleOrDefaultAsync(x => x.Id == id, cancellationToken);

            return entity ?? throw new NotFoundException(EntityName, id);
        }

        async Task<IBaseRelationalEntity?> IGenericRelationalReadOnlyRepository.SingleByIdOrDefaultAsync(Guid id, bool noTracking, CancellationToken cancellationToken)
        {
            IQueryable<TEntity> query = DbSet;

            if (noTracking)
            {
                query = query.AsNoTracking();
            }

            return await query.SingleOrDefaultAsync(x => x.Id == id, cancellationToken);
        }

        public async Task<int> GetCountAsync(CancellationToken cancellationToken = default)
        {
            return await DbSet.CountAsync(cancellationToken);
        }

        public async Task<bool> ExistsAsync(CancellationToken cancellationToken = default)
        {
            return await DbSet.AnyAsync(cancellationToken);
        }

        public async Task ExistsElseThrowAsync(CancellationToken cancellationToken = default)
        {
            if (!await DbSet.AnyAsync(cancellationToken))
            {
                throw new NotFoundException(EntityName);
            }
        }

        public async Task<bool> ExistsByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await DbSet.AnyAsync(x => x.Id == id, cancellationToken);
        }

        public async Task ExistsByIdElseThrowAsync(Guid id, CancellationToken cancellationToken = default)
        {
            if (!await DbSet.AnyAsync(x => x.Id == id, cancellationToken))
            {
                throw new NotFoundException(EntityName, id);
            }
        }
        #endregion

        #region ProjectTo
        public async Task<T> ProjectedSingleAsync<T>(Expression<Func<T, bool>>? filter = null,
                                                     bool noTracking = false,
                                                     CancellationToken cancellationToken = default)
        {
            IQueryable<TEntity> query = noTracking ? DbSet.AsNoTracking() : DbSet;

            if (filter is null)
            {
                return await query.ProjectTo<T>(Mapper.ConfigurationProvider).SingleOrDefaultAsync(cancellationToken) ?? throw new NotFoundException(EntityName);
            }

            return await query.ProjectTo<T>(Mapper.ConfigurationProvider).SingleOrDefaultAsync(filter, cancellationToken) ?? throw new NotFoundException(EntityName);
        }

        public async Task<T?> ProjectedSingleOrDefaultAsync<T>(Expression<Func<T, bool>>? filter = null,
                                                               bool noTracking = false,
                                                               CancellationToken cancellationToken = default)
        {
            IQueryable<TEntity> query = noTracking ? DbSet.AsNoTracking() : DbSet;

            if (filter is null)
            {
                return await query.ProjectTo<T>(Mapper.ConfigurationProvider).SingleOrDefaultAsync(cancellationToken);
            }

            return await query.ProjectTo<T>(Mapper.ConfigurationProvider).SingleOrDefaultAsync(filter, cancellationToken);
        }

        public async Task<T> ProjectedFirstAsync<T>(Expression<Func<T, bool>>? filter = null,
                                                    bool noTracking = false,
                                                    CancellationToken cancellationToken = default)
        {
            IQueryable<TEntity> query = noTracking ? DbSet.AsNoTracking() : DbSet;

            T? entity;
            if (filter is null)
            {
                entity = await query.ProjectTo<T>(Mapper.ConfigurationProvider).FirstOrDefaultAsync(cancellationToken);
            }
            else
            {
                entity = await query.ProjectTo<T>(Mapper.ConfigurationProvider).FirstOrDefaultAsync(filter, cancellationToken);
            }

            return entity ?? throw new NotFoundException(EntityName);
        }

        public async Task<T?> ProjectedFirstOrDefaultAsync<T>(Expression<Func<T, bool>>? filter = null,
                                                              bool noTracking = false,
                                                              CancellationToken cancellationToken = default)
        {
            IQueryable<TEntity> query = noTracking ? DbSet.AsNoTracking() : DbSet;

            if (filter is null)
            {
                return await query.ProjectTo<T>(Mapper.ConfigurationProvider).FirstOrDefaultAsync(cancellationToken);
            }

            return await query.ProjectTo<T>(Mapper.ConfigurationProvider).FirstOrDefaultAsync(filter, cancellationToken);
        }

        public async Task<T> ProjectedSingleAsync<T>(Expression<Func<TEntity, bool>> preFilter,
                                                     Expression<Func<T, bool>>? filter = null,
                                                     bool noTracking = false,
                                                     CancellationToken cancellationToken = default)
        {
            IQueryable<TEntity> query = noTracking ? DbSet.AsNoTracking() : DbSet;
            query = query.Where(preFilter);

            if (filter is null)
            {
                return await query.ProjectTo<T>(Mapper.ConfigurationProvider).SingleOrDefaultAsync(cancellationToken) ?? throw new NotFoundException(EntityName);
            }

            return await query.ProjectTo<T>(Mapper.ConfigurationProvider).SingleOrDefaultAsync(filter, cancellationToken) ?? throw new NotFoundException(EntityName);
        }

        public async Task<T?> ProjectedSingleOrDefaultAsync<T>(Expression<Func<TEntity, bool>> preFilter,
                                                               Expression<Func<T, bool>>? filter = null,
                                                               bool noTracking = false,
                                                               CancellationToken cancellationToken = default)
        {
            IQueryable<TEntity> query = noTracking ? DbSet.AsNoTracking() : DbSet;
            query = query.Where(preFilter);

            if (filter is null)
            {
                return await query.ProjectTo<T>(Mapper.ConfigurationProvider).SingleOrDefaultAsync(cancellationToken);
            }

            return await query.ProjectTo<T>(Mapper.ConfigurationProvider).SingleOrDefaultAsync(filter, cancellationToken);
        }

        public async Task<T> ProjectedFirstAsync<T>(Expression<Func<TEntity, bool>> preFilter,
                                                    Expression<Func<T, bool>>? filter = null,
                                                    bool noTracking = false,
                                                    CancellationToken cancellationToken = default)
        {
            IQueryable<TEntity> query = noTracking ? DbSet.AsNoTracking() : DbSet;
            query = query.Where(preFilter);

            T? entity;
            if (filter is null)
            {
                entity = await query.ProjectTo<T>(Mapper.ConfigurationProvider).FirstOrDefaultAsync(cancellationToken);
            }
            else
            {
                entity = await query.ProjectTo<T>(Mapper.ConfigurationProvider).FirstOrDefaultAsync(filter, cancellationToken);
            }

            return entity ?? throw new NotFoundException(EntityName);
        }

        public async Task<T?> ProjectedFirstOrDefaultAsync<T>(Expression<Func<TEntity, bool>> preFilter,
                                                              Expression<Func<T, bool>>? filter = null,
                                                              bool noTracking = false,
                                                              CancellationToken cancellationToken = default)
        {
            IQueryable<TEntity> query = noTracking ? DbSet.AsNoTracking() : DbSet;
            query = query.Where(preFilter);

            if (filter is null)
            {
                return await query.ProjectTo<T>(Mapper.ConfigurationProvider).FirstOrDefaultAsync(cancellationToken);
            }

            return await query.ProjectTo<T>(Mapper.ConfigurationProvider).FirstOrDefaultAsync(filter, cancellationToken);
        }

        public async Task<T> ProjectedSingleByIdAsync<T>(Guid id, bool noTracking = false, CancellationToken cancellationToken = default)
        {
            return await ProjectedSingleByIdOrDefaultAsync<T>(id, noTracking, cancellationToken) ?? throw new NotFoundException(EntityName, id);
        }

        public async Task<T?> ProjectedSingleByIdOrDefaultAsync<T>(Guid id, bool noTracking = false, CancellationToken cancellationToken = default)
        {
            IQueryable<TEntity> query = noTracking ? DbSet.AsNoTracking() : DbSet;

            return await query.Where(x => x.Id == id).ProjectTo<T>(Mapper.ConfigurationProvider).SingleOrDefaultAsync(cancellationToken);
        }

        public async Task<T> ProjectedSingleByIdWithRelatedAsync<T>(Guid id,
                                                                    bool noTracking = false,
                                                                    CancellationToken cancellationToken = default,
                                                                    params Expression<Func<TEntity, object>>[] relatedSelectors)
        {
            return await ProjectedSingleByIdWithRelatedOrDefaultAsync<T>(id, noTracking, cancellationToken, relatedSelectors) ?? throw new NotFoundException(EntityName);
        }

        public async Task<T?> ProjectedSingleByIdWithRelatedOrDefaultAsync<T>(Guid id,
                                                                              bool noTracking = false,
                                                                              CancellationToken cancellationToken = default,
                                                                              params Expression<Func<TEntity, object>>[] relatedSelectors)
        {
            IQueryable<TEntity> query = DbSet.AsQueryable();

            foreach (Expression<Func<TEntity, object>> relatedExpr in relatedSelectors)
            {
                query = query.Include(relatedExpr);
            }

            return await query.Where(x => x.Id == id).ProjectTo<T>(Mapper.ConfigurationProvider).SingleOrDefaultAsync(cancellationToken);
        }

        public async Task<List<T>> ProjectedAllAsync<T>(Expression<Func<TEntity, bool>>? filter = null,
                                                        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
                                                        bool noTracking = false,
                                                        int? skip = null,
                                                        int? take = null,
                                                        CancellationToken cancellationToken = default)
        {
            IQueryable<TEntity> query = GetQueryable(filter, orderBy, noTracking, skip, take);

            return await query.ProjectTo<T>(Mapper.ConfigurationProvider)
                              .ToListAsync(cancellationToken);
        }

        public async Task<List<T>> ProjectedAllWithRelatedAsync<T>(Expression<Func<TEntity, bool>>? filter = null,
                                                                Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
                                                                bool noTracking = false,
                                                                int? skip = null,
                                                                int? take = null,
                                                                CancellationToken cancellationToken = default,
                                                                params Expression<Func<TEntity, object>>[] relatedSelectors)
        {
            IQueryable<TEntity> query = GetQueryable(filter, orderBy, noTracking, skip, take);

            foreach (Expression<Func<TEntity, object>> relatedExpr in relatedSelectors)
            {
                query = query.Include(relatedExpr);
            }

            return await query.ProjectTo<T>(Mapper.ConfigurationProvider)
                             .ToListAsync(cancellationToken);
        }
        #endregion
    }
}
