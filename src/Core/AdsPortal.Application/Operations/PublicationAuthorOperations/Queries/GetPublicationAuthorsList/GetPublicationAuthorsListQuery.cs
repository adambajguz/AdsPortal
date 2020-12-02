namespace AdsPortal.Application.Operations.PublicationAuthorOperations.Queries.GetPublicationAuthorsList
{
    using System.Threading;
    using System.Threading.Tasks;
    using AdsPortal.Application.GenericHandlers.Relational.Queries;
    using AdsPortal.Application.Interfaces.Persistence.UoW;
    using AdsPortal.Domain.Entities;

    public class GetPublicationAuthorsListQuery : IGetListQuery<GetPublicationAuthorsListResponse>
    {
        public GetPublicationAuthorsListQuery()
        {

        }

        private class Handler : GetListQueryHandler<GetPublicationAuthorsListQuery, PublicationAuthor, GetPublicationAuthorsListResponse>
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
