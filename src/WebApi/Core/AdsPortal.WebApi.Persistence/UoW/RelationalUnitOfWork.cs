namespace AdsPortal.WebApi.Persistence.UoW
{
    using System;
    using AdsPortal.WebApi.Application.Interfaces.Identity;
    using AdsPortal.WebApi.Application.Interfaces.Persistence.Repository;
    using AdsPortal.WebApi.Application.Interfaces.Persistence.UoW;
    using AdsPortal.WebApi.Persistence.Interfaces.DbContext;
    using AdsPortal.WebApi.Persistence.Repository;
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

        public RelationalUnitOfWork(ICurrentUserService currentUserService, IRelationalDbContext context, IMapper mapper) :
            base(currentUserService, context, mapper)
        {
            //Data
            _authors = new Lazy<IAdvertisementsRepository>(() => GetSpecificRepository<IAdvertisementsRepository, AdvertisementsRepository>());
            _journals = new Lazy<ICategoriesRepository>(() => GetSpecificRepository<ICategoriesRepository, CategoriesRepository>());

            //Media
            _mediaItems = new Lazy<IMediaItemsRepository>(() => GetSpecificRepository<IMediaItemsRepository, MediaItemsRepository>());

            //Identity
            _users = new Lazy<IUsersRepository>(() => GetSpecificRepository<IUsersRepository, UsersRepository>());

            //Jobs
            _jobs = new Lazy<IJobsRepository>(() => GetSpecificRepository<IJobsRepository, JobsRepository>());
        }
    }
}
