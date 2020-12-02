namespace AdsPortal.Application.Interfaces.Persistence.Repository
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using AdsPortal.Application.Interfaces.Persistence.Repository.Generic;
    using Domain.Entities;

    public interface IAdvertisementsRepository : IGenericRelationalRepository<Advertisement>
    {
        public Task<List<Advertisement>> AllAsync_WithRelated(bool noTracking = false,
                                                       int? skip = null,
                                                       int? take = null,
                                                       CancellationToken cancellationToken = default);
    }
}
