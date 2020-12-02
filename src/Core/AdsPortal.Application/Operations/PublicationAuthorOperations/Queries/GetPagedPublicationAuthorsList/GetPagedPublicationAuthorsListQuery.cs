namespace AdsPortal.Application.Operations.PublicationAuthorOperations.Queries.GetPagedPublicationAuthorsList
{
    using System.Threading;
    using System.Threading.Tasks;
    using AdsPortal.Application.GenericHandlers.Relational.Queries;
    using AdsPortal.Application.Interfaces.Persistence.UoW;
    using AdsPortal.Application.Operations.PublicationAuthorOperations.Queries.GetPublicationAuthorsList;
    using AdsPortal.Domain.Entities;

    public class GetPagedPublicationAuthorsListQuery : IGetPagedListQuery<GetPublicationAuthorsListResponse>
    {
        public int Page { get; set; }
        public int EntiresPerPage { get; set; }

        private class Handler : GetPagedListQueryHandler<GetPagedPublicationAuthorsListQuery, PublicationAuthor, GetPublicationAuthorsListResponse>
        {
            public Handler(IAppRelationalUnitOfWork uow) : base(uow)
            {

            }

            protected override Task OnInit(CancellationToken cancellationToken)
            {
                return Task.CompletedTask;
            }
        }
    }
}
