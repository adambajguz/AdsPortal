namespace AdsPortal.Application.Operations.AuthorOperations.Queries.GetAuthorsList
{
    using System.Threading;
    using System.Threading.Tasks;
    using AdsPortal.Application.GenericHandlers.Relational.Queries;
    using AdsPortal.Application.Interfaces.Persistence.UoW;
    using AdsPortal.Domain.Entities;

    public class GetAuthorsListQuery : IGetListQuery<GetAuthorsListResponse>
    {
        public GetAuthorsListQuery()
        {

        }

        private class Handler : GetListQueryHandler<GetAuthorsListQuery, Author, GetAuthorsListResponse>
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

