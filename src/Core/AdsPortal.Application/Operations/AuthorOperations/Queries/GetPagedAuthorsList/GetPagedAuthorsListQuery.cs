namespace AdsPortal.Application.Operations.AuthorOperations.Queries.GetPagedAuthorsList
{
    using System.Threading;
    using System.Threading.Tasks;
    using AdsPortal.Application.GenericHandlers.Relational.Queries;
    using AdsPortal.Application.Interfaces.Persistence.UoW;
    using AdsPortal.Application.Operations.AuthorOperations.Queries.GetAuthorsList;
    using AdsPortal.Domain.Entities;

    public class GetPagedAuthorsListQuery : IGetPagedListQuery<GetAuthorsListResponse>
    {
        public int Page { get; set; }
        public int EntiresPerPage { get; set; }

        private class Handler : GetPagedListQueryHandler<GetPagedAuthorsListQuery, Author, GetAuthorsListResponse>
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
