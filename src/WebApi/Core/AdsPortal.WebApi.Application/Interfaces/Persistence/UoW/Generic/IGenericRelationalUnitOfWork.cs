namespace AdsPortal.WebApi.Application.Interfaces.Persistence.UoW.Generic
{
    using System.Threading;
    using System.Threading.Tasks;
    using AdsPortal.WebApi.Application.Interfaces.Persistence.Repository.Generic;
    using AdsPortal.WebApi.Domain.Abstractions.Base;

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
