namespace AdsPortal.Application.Interfaces.Persistence.UoW
{
    using AdsPortal.Application.Interfaces.Persistence.Repository;
    using AdsPortal.Application.Interfaces.Persistence.UoW.Generic;

    public interface IAppRelationalUnitOfWork : IGenericAuditableRelationalUnitOfWork
    {
        //Data
        IAuthorsRepository Authors { get; }
        IDegreesRepository Degrees { get; }
        IDepartmentsRepository Departments { get; }
        IJournalsRepository Journals { get; }
        IPublicationsRepository Publications { get; }
        IPublicationAuthorsRepository PublicationAuthors { get; }

        //Media
        IMediaItemsRepository MediaItems { get; }

        //Identity
        IUsersRepository Users { get; }

        //Jobs
        IJobsRepository Jobs { get; }
    }
}
