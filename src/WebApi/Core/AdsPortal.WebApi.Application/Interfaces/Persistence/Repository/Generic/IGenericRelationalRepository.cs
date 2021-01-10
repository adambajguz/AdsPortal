namespace AdsPortal.WebApi.Application.Interfaces.Persistence.Repository.Generic
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using AdsPortal.WebApi.Domain.Abstractions.Base;

    public interface IGenericRelationalRepository : IGenericRelationalReadOnlyRepository
    {
        IBaseRelationalEntity Add(IBaseRelationalEntity entity);
        void Update(IBaseRelationalEntity entity);
        Task RemoveByIdAsync(Guid id);
        void Remove(IBaseRelationalEntity entity);

        void AddMultiple(IEnumerable<IBaseRelationalEntity> entities);
        void UpdateMultiple(IEnumerable<IBaseRelationalEntity> entities);
        Task RemoveMultipleByIdAsync(IEnumerable<Guid> ids);
        void RemoveMultiple(IEnumerable<IBaseRelationalEntity> entities);
    }

    public interface IGenericRelationalRepository<TEntity> : IGenericRelationalRepository, IGenericRelationalReadOnlyRepository<TEntity>
        where TEntity : class, IBaseRelationalEntity
    {
        TEntity Add(TEntity entity);
        void EnsureTracked(TEntity entity);
        void Update(TEntity entity, bool force = false);
        void Remove(TEntity entity);

        void AddMultiple(IEnumerable<TEntity> entities);
        void EnsureTrackedMultiple(IEnumerable<TEntity> entities);
        void UpdateMultiple(IEnumerable<TEntity> entities);
        void RemoveMultiple(IEnumerable<TEntity> entities);
    }
}