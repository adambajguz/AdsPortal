namespace AdsPortal.WebApi.Persistence.Repository.Generic
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using AdsPortal.Shared.Extensions.Extensions;
    using AdsPortal.WebApi.Application.Interfaces.Identity;
    using AdsPortal.WebApi.Application.Interfaces.Persistence.Repository.Generic;
    using AdsPortal.WebApi.Domain.Abstractions.Base;
    using AdsPortal.WebApi.Persistence.Interfaces.DbContext.Generic;
    using AutoMapper;
    using Microsoft.EntityFrameworkCore.ChangeTracking;

    public class GenericRelationalRepository<TEntity> : GenericReadOnlyRelationalRepository<TEntity>, IGenericRelationalRepository<TEntity>
        where TEntity : class, IBaseRelationalEntity
    {
        private readonly ICurrentUserService _currentUser;

        public GenericRelationalRepository(ICurrentUserService currentUserService, IGenericRelationalDbContext dbContext, IMapper mapper) : base(dbContext, mapper)
        {
            _currentUser = currentUserService;
        }

        #region IGenericRelationalRepository<TEntity>
        #region Single
        public virtual TEntity Add(TEntity entity)
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

            EntityEntry<TEntity> createdEntity = DbSet.Add(entity);

            return createdEntity.Entity;
        }

        public virtual void EnsureTracked(TEntity entity)
        {
            bool tracking = Provider.ChangeTracker.Entries<TEntity>().Any(x => x.Entity.Id == entity.Id);

            if (!tracking)
            {
                DbSet.Attach(entity);
            }
        }

        public virtual void Update(TEntity entity, bool force = false)
        {
            if (entity is IEntityLastSaved entityModification)
            {
                entityModification.LastSavedOn = DateTime.UtcNow;
                entityModification.LastSavedBy = _currentUser.UserId;
            }

            //Entity may not be tracked so we need to check that
            bool tracking = Provider.ChangeTracker.Entries<TEntity>().Any(x => x.Entity.Id == entity.Id);

            if (!tracking)
            {
                //For untracked entity there are two approches:
                // 1. DbSet.Update(entity) can be called to ensure an update will be performed.
                // 2. exception can be thrown because if we don't trust the programmer that the entity we could attach is valid, i.e. won't revert columns to default values.

                if (force)
                {
                    DbSet.Update(entity);
                }
                else
                {
                    throw new InvalidOperationException($"Cannot update untracked {typeof(TEntity).FullName} with id {entity.Id}. " +
                                                        $"You can use {nameof(force)} parameter if you realy want to update entity with all properties override method.");
                }
            }
        }

        public virtual void Remove(TEntity entity)
        {
            DbSet.Remove(entity);
        }
        #endregion

        #region Multiple
        public void AddMultiple(IEnumerable<TEntity> entities)
        {
            entities.ForEachInParallel((entity) =>
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
            });

            DbSet.AddRange(entities);
        }

        public virtual void EnsureTrackedMultiple(IEnumerable<TEntity> entities)
        {
            foreach (TEntity entity in entities)
            {
                EnsureTracked(entity);
            }
        }

        public void UpdateMultiple(IEnumerable<TEntity> entities)
        {
            entities.ForEachInParallel((entity) =>
            {
                if (entity is IEntityLastSaved entityModification)
                {
                    entityModification.LastSavedOn = DateTime.UtcNow;
                    entityModification.LastSavedBy = _currentUser.UserId;
                }

                Update(entity);
            });

            //DbSet.UpdateRange(entities);
        }

        public void RemoveMultiple(IEnumerable<TEntity> entities)
        {
            DbSet.RemoveRange(entities);
        }
        #endregion
        #endregion

        #region IGenericRelationalRepository
        #region Single
        public IBaseRelationalEntity Add(IBaseRelationalEntity entity)
        {
            if (entity is TEntity e)
            {
                return Add(e);
            }

            throw new ArgumentException($"Entity is not of type {typeof(TEntity).Name}", nameof(entity));
        }

        public void Update(IBaseRelationalEntity entity)
        {
            if (entity is TEntity e)
            {
                Update(e);
            }
            else
            {
                throw new ArgumentException($"Entity is not of type {typeof(TEntity).Name}", nameof(entity));
            }
        }
        public virtual async Task RemoveByIdAsync(Guid id)
        {
            TEntity entity = await DbSet.FindAsync(id);
            DbSet.Remove(entity);
        }

        public void Remove(IBaseRelationalEntity entity)
        {
            if (entity is TEntity e)
            {
                Remove(e);
            }
            else
            {
                throw new ArgumentException($"Entity is not of type {typeof(TEntity).Name}", nameof(entity));
            }
        }
        #endregion

        #region Multiple
        public void AddMultiple(IEnumerable<IBaseRelationalEntity> entities)
        {
            AddMultiple(entities);
        }

        public void UpdateMultiple(IEnumerable<IBaseRelationalEntity> entities)
        {
            UpdateMultiple(entities);
        }

        public async Task RemoveMultipleByIdAsync(IEnumerable<Guid> entities)
        {
            List<TEntity> entitiesToRemove = new List<TEntity>();
            foreach (Guid id in entities)
            {
                TEntity entity = await DbSet.FindAsync(id);
                entitiesToRemove.Add(entity);
            }

            DbSet.RemoveRange(entitiesToRemove);
        }

        public void RemoveMultiple(IEnumerable<IBaseRelationalEntity> entities)
        {
            RemoveMultiple(entities);
        }
        #endregion
        #endregion
    }
}
