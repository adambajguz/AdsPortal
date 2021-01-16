namespace AdsPortal.WebApi.Persistence.UoW
{
    using System;
    using AdsPortal.WebApi.Application.Interfaces.Identity;
    using AdsPortal.WebApi.Application.Interfaces.Persistence;
    using AdsPortal.WebApi.Domain.Interfaces.Repository;
    using AdsPortal.WebApi.Domain.Interfaces.UoW;
    using AdsPortal.WebApi.Persistence.Interfaces.DbContext;
    using AdsPortal.WebApi.Persistence.UoW.Generic;
    using AutoMapper;

    public class RelationalUnitOfWork : GenericAuditableRelationalUnitOfWork, IAppRelationalUnitOfWork
    {
        //Data
        private readonly Lazy<IAdvertisementsRepository> _authors;
        public IAdvertisementsRepository Advertisements => _authors.Value;

        private readonly Lazy<ICategoriesRepository> _journals;
        public ICategoriesRepository Categories => _journals.Value;

        //Media
        private readonly Lazy<IMediaItemsRepository> _mediaItems;
        public IMediaItemsRepository MediaItems => _mediaItems.Value;

        //Identity
        private readonly Lazy<IUsersRepository> _users;
        public IUsersRepository Users => _users.Value;

        //Jobs
        private readonly Lazy<IJobsRepository> _jobs;
        public IJobsRepository Jobs => _jobs.Value;

        public RelationalUnitOfWork(IRepositoryFactory repositoryFactory, ICurrentUserService currentUserService, IRelationalDbContext context, IMapper mapper) :
            base(repositoryFactory, currentUserService, context, mapper)
        {
            //Data
            _authors = new Lazy<IAdvertisementsRepository>(() => GetSpecificRepository<IAdvertisementsRepository>());
            _journals = new Lazy<ICategoriesRepository>(() => GetSpecificRepository<ICategoriesRepository>());

            //Media
            _mediaItems = new Lazy<IMediaItemsRepository>(() => GetSpecificRepository<IMediaItemsRepository>());

            //Identity
            _users = new Lazy<IUsersRepository>(() => GetSpecificRepository<IUsersRepository>());

            //Jobs
            _jobs = new Lazy<IJobsRepository>(() => GetSpecificRepository<IJobsRepository>());
        }
    }
}
