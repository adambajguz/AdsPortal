namespace AdsPortal.WebApi.Application.Interfaces.Persistence.UoW
{
    using AdsPortal.WebApi.Application.Interfaces.Persistence.Repository;
    using AdsPortal.WebApi.Application.Interfaces.Persistence.UoW.Generic;

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
