namespace AdsPortal.Application.Interfaces.Persistence.UoW.Generic
{
    using System.Threading;
    using System.Threading.Tasks;
    using AdsPortal.Application.Interfaces.Persistence.Repository.Generic;
    using AdsPortal.Domain.Abstractions.Base;

    public interface IGenericMongoUnitOfWork
    {
        IGenericMongoRepository<TEntity> GetRepository<TEntity>()
            where TEntity : class, IBaseMongoEntity;

        IGenericMongoReadOnlyRepository<TEntity> GetReadOnlyRepository<TEntity>()
           where TEntity : class, IBaseMongoEntity;

        int SaveChanges();
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}

