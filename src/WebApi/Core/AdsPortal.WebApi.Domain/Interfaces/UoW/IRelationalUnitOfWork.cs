namespace AdsPortal.WebApi.Domain.Interfaces.UoW
{
    using AdsPortal.WebApi.Domain.Interfaces.Repository;
    using AdsPortal.WebApi.Domain.Interfaces.UoW.Generic;

    public interface IAppRelationalUnitOfWork : IGenericAuditableRelationalUnitOfWork
    {
        //Data
        IAdvertisementsRepository Advertisements { get; }
        ICategoriesRepository Categories { get; }

        //Media
        IMediaItemsRepository MediaItems { get; }

        //Identity
        IUsersRepository Users { get; }

        //Jobs
        IJobsRepository Jobs { get; }
    }
}
