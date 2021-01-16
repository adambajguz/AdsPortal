namespace AdsPortal.WebApi.Domain.Interfaces.UoW.Generic
{
    using System.Threading;
    using System.Threading.Tasks;
    using AdsPortal.WebApi.Domain.Abstractions.Base;
    using AdsPortal.WebApi.Domain.Interfaces.Repository.Generic;

    public interface IGenericRelationalUnitOfWork
    {
        IGenericRelationalReadOnlyRepository GetReadOnlyRepositoryByName(string tableName);

        IGenericRelationalReadOnlyRepository<TEntity> GetReadOnlyRepository<TEntity>()
            where TEntity : class, IBaseRelationalEntity;

        IGenericRelationalRepository GetRepositoryByName(string tableName);

        IGenericRelationalRepository<TEntity> GetRepository<TEntity>()
            where TEntity : class, IBaseRelationalEntity;
        TSpecificRepositoryInterface GetSpecificRepository<TSpecificRepositoryInterface>()
            where TSpecificRepositoryInterface : IGenericRelationalReadOnlyRepository;

        int SaveChanges();
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
