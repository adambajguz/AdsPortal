namespace AdsPortal.WebApi.Persistence.Repository
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using AdsPortal.WebApi.Application.Interfaces.Identity;
    using AdsPortal.WebApi.Domain.Entities;
    using AdsPortal.WebApi.Domain.Interfaces.Repository;
    using AdsPortal.WebApi.Persistence.Interfaces.DbContext;
    using AdsPortal.WebApi.Persistence.Repository.Generic;
    using AutoMapper;
    using Microsoft.EntityFrameworkCore;

    public class AdvertisementsRepository : GenericRelationalRepository<Advertisement>, IAdvertisementsRepository
    {
        public AdvertisementsRepository(ICurrentUserService currentUserService,
                                        IRelationalDbContext context,
                                        IMapper mapper) : base(currentUserService, context, mapper)
        {

        }

        public async Task<List<Advertisement>> AllAsync_WithRelated(bool noTracking = false,
                                                                    int? skip = null,
                                                                    int? take = null,
                                                                    CancellationToken cancellationToken = default)
        {
            return await GetQueryable(null, null, noTracking, skip, take).Include(x => x.Category)
                                                                         .Include(x => x.Author)
                                                                         .ToListAsync(cancellationToken);
        }
    }
}
