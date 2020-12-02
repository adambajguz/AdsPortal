namespace AdsPortal.Application.Interfaces.Persistence.Repository
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using AdsPortal.Application.Interfaces.Persistence.Repository.Generic;
    using Domain.Entities;

    public interface IPublicationsRepository : IGenericRelationalRepository<Publication>
    {
        Task<Publication> SingleById_DetailsWithRelated(Guid id,
                                                        bool noTracking = false,
                                                        CancellationToken cancellationToken = default);
    }
}
