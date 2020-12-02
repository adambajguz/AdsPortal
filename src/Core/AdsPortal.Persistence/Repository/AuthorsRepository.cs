namespace AdsPortal.Persistence.Repository
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using AutoMapper;
    using AdsPortal.Application.Interfaces.Identity;
    using AdsPortal.Application.Interfaces.Persistence.Repository;
    using AdsPortal.Domain.Entities;
    using AdsPortal.Persistence.Interfaces.DbContext;
    using AdsPortal.Persistence.Repository.Generic;
    using Microsoft.EntityFrameworkCore;

    public class AuthorsRepository : GenericRelationalRepository<Author>, IAuthorsRepository
    {
        public AuthorsRepository(ICurrentUserService currentUserService,
                                 IRelationalDbContext context,
                                 IMapper mapper) : base(currentUserService, context, mapper)
        {

        }

        public async Task<List<Author>> AllAsync_WithRelated(bool noTracking = false,
                                                             int? skip = null,
                                                             int? take = null,
                                                             CancellationToken cancellationToken = default)
        {
            return await GetQueryable(null, null, noTracking, skip, take).Include(x => x.PublicationAuthors)
                                                                         .ThenInclude(x => x.Publication)
                                                                         .ThenInclude(x => x.Journal)
                                                                         .Include(x => x.Department)
                                                                         .ToListAsync(cancellationToken);
        }
    }
}
