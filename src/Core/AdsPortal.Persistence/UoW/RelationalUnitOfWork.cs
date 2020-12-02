namespace AdsPortal.Persistence.UoW
{
    using System;
    using AdsPortal.Application.Interfaces.Identity;
    using AdsPortal.Application.Interfaces.Persistence.Repository;
    using AdsPortal.Application.Interfaces.Persistence.UoW;
    using AdsPortal.Persistence.Interfaces.DbContext;
    using AdsPortal.Persistence.Repository;
    using AdsPortal.Persistence.UoW.Generic;
    using AutoMapper;

    public class RelationalUnitOfWork : GenericAuditableRelationalUnitOfWork, IAppRelationalUnitOfWork
    {
        //Data
        private readonly Lazy<IAuthorsRepository> _authors;
        public IAuthorsRepository Authors => _authors.Value;

        private readonly Lazy<IDegreesRepository> _degrees;
        public IDegreesRepository Degrees => _degrees.Value;

        private readonly Lazy<IDepartmentsRepository> _departments;
        public IDepartmentsRepository Departments => _departments.Value;

        private readonly Lazy<IJournalsRepository> _journals;
        public IJournalsRepository Journals => _journals.Value;

        private readonly Lazy<IPublicationsRepository> _publications;
        public IPublicationsRepository Publications => _publications.Value;

        private readonly Lazy<IPublicationAuthorsRepository> _publicationAuthors;
        public IPublicationAuthorsRepository PublicationAuthors => _publicationAuthors.Value;

        //Media
        private readonly Lazy<IMediaItemsRepository> _mediaItems;
        public IMediaItemsRepository MediaItems => _mediaItems.Value;

        //Identity
        private readonly Lazy<IUsersRepository> _users;
        public IUsersRepository Users => _users.Value;

        //Jobs
        private readonly Lazy<IJobsRepository> _jobs;
        public IJobsRepository Jobs => _jobs.Value;

        public RelationalUnitOfWork(ICurrentUserService currentUserService, IRelationalDbContext context, IMapper mapper) :
            base(currentUserService, context, mapper)
        {
            //Data
            _authors = new Lazy<IAuthorsRepository>(() => GetSpecificRepository<IAuthorsRepository, AuthorsRepository>());
            _degrees = new Lazy<IDegreesRepository>(() => GetSpecificRepository<IDegreesRepository, DegreesRepository>());
            _departments = new Lazy<IDepartmentsRepository>(() => GetSpecificRepository<IDepartmentsRepository, DepartmentsRepository>());
            _journals = new Lazy<IJournalsRepository>(() => GetSpecificRepository<IJournalsRepository, JournalsRepository>());
            _publications = new Lazy<IPublicationsRepository>(() => GetSpecificRepository<IPublicationsRepository, PublicationsRepository>());
            _publicationAuthors = new Lazy<IPublicationAuthorsRepository>(() => GetSpecificRepository<IPublicationAuthorsRepository, PublicationAuthorsRepository>());

            //Media
            _mediaItems = new Lazy<IMediaItemsRepository>(() => GetSpecificRepository<IMediaItemsRepository, MediaItemsRepository>());

            //Identity
            _users = new Lazy<IUsersRepository>(() => GetSpecificRepository<IUsersRepository, UsersRepository>());

            //Jobs
            _jobs = new Lazy<IJobsRepository>(() => GetSpecificRepository<IJobsRepository, JobsRepository>());
        }
    }
}
