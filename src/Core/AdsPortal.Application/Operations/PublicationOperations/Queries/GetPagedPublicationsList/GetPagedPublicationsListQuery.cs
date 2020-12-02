namespace AdsPortal.Application.Operations.PublicationOperations.Queries.GetPagedPublicationsList
{
    using System.Threading;
    using System.Threading.Tasks;
    using AdsPortal.Application.GenericHandlers.Relational.Queries;
    using AdsPortal.Application.Interfaces.Persistence.UoW;
    using AdsPortal.Application.Operations.PublicationOperations.Queries.GetPublicationsList;
    using AdsPortal.Domain.Entities;

    public class GetPagedPublicationsListQuery : IGetPagedListQuery<GetPublicationsListResponse>
    {
        public int Page { get; set; }
        public int EntiresPerPage { get; set; }

        private class Handler : GetPagedListQueryHandler<GetPagedPublicationsListQuery, Journal, GetPublicationsListResponse>
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
