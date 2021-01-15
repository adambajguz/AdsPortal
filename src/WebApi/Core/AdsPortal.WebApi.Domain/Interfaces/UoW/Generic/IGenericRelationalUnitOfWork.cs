namespace AdsPortal.WebApi.Domain.Interfaces.UoW.Generic
{
    using System.Threading;
    using System.Threading.Tasks;
    using AdsPortal.WebApi.Domain.Abstractions.Base;
    using AdsPortal.WebApi.Domain.Interfaces.Repository.Generic;

    public interface IGenericRelationalUnitOfWork
    {
        IGenericRelationalRepository GetRepositoryByName(string name);

        IGenericRelationalRepository<TEntity> GetRepository<TEntity>()
            where TEntity : class, IBaseRelationalEntity;

        IGenericRelationalReadOnlyRepository GetReadOnlyRepositoryByName(string name);

        IGenericRelationalReadOnlyRepository<TEntity> GetReadOnlyRepository<TEntity>()
            where TEntity : class, IBaseRelationalEntity;

        int SaveChanges();
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
