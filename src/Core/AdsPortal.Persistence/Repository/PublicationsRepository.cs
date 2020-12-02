namespace AdsPortal.Persistence.Repository
{
    using System;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using AutoMapper;
    using AdsPortal.Application.Exceptions;
    using AdsPortal.Application.Interfaces.Identity;
    using AdsPortal.Application.Interfaces.Persistence.Repository;
    using AdsPortal.Domain.Entities;
    using AdsPortal.Persistence.Interfaces.DbContext;
    using AdsPortal.Persistence.Repository.Generic;
    using Microsoft.EntityFrameworkCore;

    public class PublicationsRepository : GenericRelationalRepository<Publication>, IPublicationsRepository
    {
        //TODO: maybe do not force user to manualy inject di services
        public PublicationsRepository(ICurrentUserService currentUserService,
                                      IRelationalDbContext context,
                                      IMapper mapper) : base(currentUserService, context, mapper)
        {

        }

        public async Task<Publication> SingleById_DetailsWithRelated(Guid id, bool noTracking = false, CancellationToken cancellationToken = default)
        {
            IQueryable<Publication> query = noTracking ? DbSet.AsNoTracking() : DbSet;

            Publication? entity = await query.Include(x => x.Journal)
                                             .Include(x => x.PublicationAuthors)
                                             .ThenInclude(x => x.Author)
                                             .SingleOrDefaultAsync(x => x.Id == id, cancellationToken);

            return entity ?? throw new NotFoundException(EntityName, id);
        }
    }
}
