namespace AdsPortal.WebApi.Persistence.Repository.Generic
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using AdsPortal.Shared.Extensions.Extensions;
    using AdsPortal.WebApi.Application.Interfaces.Identity;
    using AdsPortal.WebApi.Application.Interfaces.Persistence.Repository.Generic;
    using AdsPortal.WebApi.Domain.Abstractions.Base;
    using AdsPortal.WebApi.Persistence.Interfaces.DbContext.Generic;
    using AutoMapper;
    using Microsoft.EntityFrameworkCore;
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

        public virtual void Update(TEntity entity)
        {
            if (entity is IEntityLastSaved entityModification)
            {
                entityModification.LastSavedOn = DateTime.UtcNow;
                entityModification.LastSavedBy = _currentUser.UserId;
            }

            DbSet.Attach(entity);
        }

        public virtual void Remove(TEntity entity)
        {
            if (Provider.Entry(entity).State == EntityState.Detached)
                DbSet.Attach(entity);

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

        public void UpdateMultiple(IEnumerable<TEntity> entities)
        {
            entities.ForEachInParallel((entity) =>
            {
                if (entity is IEntityLastSaved entityModification)
                {
                    entityModification.LastSavedOn = DateTime.UtcNow;
                    entityModification.LastSavedBy = _currentUser.UserId;
                }
            });

            DbSet.UpdateRange(entities);
        }

        public void RemoveMultiple(IEnumerable<TEntity> entities)
        {
            entities.ForEachInParallel((entity) =>
            {
                if (Provider.Entry(entity).State == EntityState.Detached)
                    DbSet.Attach(entity);
            });

            DbSet.RemoveRange(entities);
        }
        #endregion
        #endregion

        #region IGenericRelationalRepository
        #region Single
        public IBaseRelationalEntity Add(IBaseRelationalEntity entity)
        {
            if (entity is TEntity e)
                return Add(e);

            throw new ArgumentException($"Entity is not of type {typeof(TEntity).Name}", nameof(entity));
        }

        public void Update(IBaseRelationalEntity entity)
        {
            if (entity is TEntity e)
                Update(e);
            else
                throw new ArgumentException($"Entity is not of type {typeof(TEntity).Name}", nameof(entity));
        }
        public virtual async Task RemoveByIdAsync(Guid id)
        {
            TEntity entity = await DbSet.FindAsync(id);
            DbSet.Remove(entity);
        }

        public void Remove(IBaseRelationalEntity entity)
        {
            if (entity is TEntity e)
                Remove(e);
            else
                throw new ArgumentException($"Entity is not of type {typeof(TEntity).Name}", nameof(entity));
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
