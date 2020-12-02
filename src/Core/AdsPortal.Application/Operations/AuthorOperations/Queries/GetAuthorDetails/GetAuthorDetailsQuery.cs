namespace AdsPortal.Application.Operations.AuthorOperations.Queries.GetAuthorDetails
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using AdsPortal.Application.GenericHandlers.Relational.Queries;
    using AdsPortal.Application.Interfaces.Persistence.UoW;
    using AdsPortal.Domain.Entities;
    using AutoMapper;

    public class GetAuthorDetailsQuery : IGetDetailsByIdQuery<GetAuthorDetailsResponse>
    {
        public Guid Id { get; }

        public GetAuthorDetailsQuery(Guid id)
        {
            Id = id;
        }

        private class Handler : GetDetailsByIdQueryHandler<GetAuthorDetailsQuery, Author, GetAuthorDetailsResponse>
        {
            public Handler(IAppRelationalUnitOfWork uow, IMapper mapper) : base(uow, mapper)
            {

            }

            protected override Task OnInit(CancellationToken cancellationToken)
            {
                return Task.CompletedTask;
            }

            protected override async Task<Author> OnFetch(CancellationToken cancellationToken)
            {
                return await Repository.SingleByIdWithRelatedAsync(Query.Id,
                                                                   noTracking: true,
                                                                   cancellationToken,
                                                                   x => x.Degree,
                                                                   x => x.Department);
            }
        }
    }
}
