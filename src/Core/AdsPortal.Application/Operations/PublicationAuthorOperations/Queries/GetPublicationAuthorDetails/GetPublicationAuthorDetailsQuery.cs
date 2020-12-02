namespace AdsPortal.Application.Operations.PublicationAuthorOperations.Queries.GetPublicationAuthorDetails
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using AutoMapper;
    using AdsPortal.Application.GenericHandlers.Relational.Queries;
    using AdsPortal.Application.Interfaces.Persistence.UoW;
    using AdsPortal.Domain.Entities;

    public class GetPublicationAuthorDetailsQuery : IGetDetailsByIdQuery<GetPublicationAuthorDetailsResponse>
    {
        public Guid Id { get; }

        public GetPublicationAuthorDetailsQuery(Guid id)
        {
            Id = id;
        }

        private class Handler : GetDetailsByIdQueryHandler<GetPublicationAuthorDetailsQuery, PublicationAuthor, GetPublicationAuthorDetailsResponse>
        {
            public Handler(IAppRelationalUnitOfWork uow, IMapper mapper) : base(uow, mapper)
            {

            }

            protected override Task OnInit(CancellationToken cancellationToken)
            {
                return Task.CompletedTask;
            }

            protected override async Task<PublicationAuthor> OnFetch(CancellationToken cancellationToken)
            {
                return await Repository.SingleByIdWithRelatedAsync(Query.Id,
                                                                   noTracking: true,
                                                                   cancellationToken,
                                                                   x => x.Author,
                                                                   x => x.Publication);
            }
        }
    }
}
