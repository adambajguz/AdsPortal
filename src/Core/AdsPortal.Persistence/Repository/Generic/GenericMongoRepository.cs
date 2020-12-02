namespace AdsPortal.Persistence.Repository.Generic
{
    using System;
    using System.Linq.Expressions;
    using System.Threading;
    using System.Threading.Tasks;
    using AdsPortal.Application.Interfaces.Identity;
    using AdsPortal.Application.Interfaces.Persistence.Repository.Generic;
    using AdsPortal.Domain.Abstractions.Base;
    using AdsPortal.Persistence.Interfaces.DbContext.Generic;
    using AutoMapper;
    using MongoDB.Driver;

    public class GenericMongoRepository<TEntity> : GenericMongoReadOnlyRepository<TEntity>, IGenericMongoRepository<TEntity>
        where TEntity : class, IBaseMongoEntity
    {
        private readonly ICurrentUserService _currentUser;

        public GenericMongoRepository(ICurrentUserService currentUserService, IGenericMongoDatabaseContext context, IMapper mapper) : base(context, mapper)
        {
            _currentUser = currentUserService;
        }

        public virtual async Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            DateTime time = DateTime.UtcNow;
            Guid? userGuid = _currentUser.UserId;

            if (entity is IEntityCreation entityCreation)
            {
                entityCreation.CreatedOn = time;
                entityCreation.CreatedBy = userGuid;
            }

            if (entity is IEntityLastSaved entityModification)
            {
                entityModification.LastSavedOn = time;
                entityModification.LastSavedBy = userGuid;
            }

            await DbSet.InsertOneAsync(entity, cancellationToken: cancellationToken);

            return entity;
        }

        public virtual async Task UpdateAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            if (entity is IEntityLastSaved entityModification)
            {
                entityModification.LastSavedOn = DateTime.UtcNow;
                entityModification.LastSavedBy = _currentUser.UserId;
            }

            //FilterDefinition<TEntity> idFilter = Builders<TEntity>.Filter.Eq(x => x.Id, entity.Id);
            await DbSet.ReplaceOneAsync(x => x.Id == entity.Id, entity, cancellationToken: cancellationToken);
        }

        public virtual async Task RemoveAsync(Guid id, CancellationToken cancellationToken = default)
        {
            FilterDefinition<TEntity> idFilter = Builders<TEntity>.Filter.Eq(x => x.Id, id);

            await DbSet.DeleteOneAsync(x => x.Id == id, cancellationToken);
        }

        public virtual async Task RemoveAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            await DbSet.DeleteOneAsync(x => x.Id == entity.Id, cancellationToken);
        }

        public virtual async Task RemoveAsync(Expression<Func<TEntity, bool>> filter, CancellationToken cancellationToken = default)
        {
            await DbSet.DeleteOneAsync(filter, cancellationToken);
        }

        public virtual async Task RemoveManyAsync(Expression<Func<TEntity, bool>> filter, CancellationToken cancellationToken = default)
        {
            await DbSet.DeleteManyAsync<TEntity>(filter, cancellationToken);
        }
    }
}
