namespace AdsPortal.Application.Interfaces.Persistence.Repository.Generic
{
    using System;
    using System.Linq.Expressions;
    using System.Threading;
    using System.Threading.Tasks;
    using AdsPortal.Domain.Abstractions.Base;

    public interface IGenericMongoRepository
    {
        Task RemoveAsync(Guid id, CancellationToken cancellationToken = default);
    }

    public interface IGenericMongoRepository<TEntity> : IGenericMongoRepository, IGenericMongoReadOnlyRepository<TEntity>
        where TEntity : class, IBaseMongoEntity
    {
        Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken = default);
        Task UpdateAsync(TEntity entity, CancellationToken cancellationToken = default);

        Task RemoveAsync(TEntity entity, CancellationToken cancellationToken = default);
        Task RemoveAsync(Expression<Func<TEntity, bool>> filter, CancellationToken cancellationToken = default);

        Task RemoveManyAsync(Expression<Func<TEntity, bool>> filter, CancellationToken cancellationToken = default);
    }
}