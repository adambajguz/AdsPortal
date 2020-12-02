namespace AdsPortal.Application.Operations.DegreeOperations.Queries.GetDegreesList
{
    using System.Threading;
    using System.Threading.Tasks;
    using AdsPortal.Application.GenericHandlers.Relational.Queries;
    using AdsPortal.Application.Interfaces.Persistence.UoW;
    using AdsPortal.Domain.Entities;

    public class GetDegreesListQuery : IGetListQuery<GetDegreesListResponse>
    {
        public GetDegreesListQuery()
        {

        }

        private class Handler : GetListQueryHandler<GetDegreesListQuery, Degree, GetDegreesListResponse>
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

