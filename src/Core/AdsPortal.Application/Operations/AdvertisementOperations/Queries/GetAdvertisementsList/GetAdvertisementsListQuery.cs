namespace AdsPortal.Application.Operations.AdvertisementOperations.Queries.GetAdvertisementsList
{
    using System.Threading;
    using System.Threading.Tasks;
    using AdsPortal.Application.GenericHandlers.Relational.Queries;
    using AdsPortal.Application.Interfaces.Persistence.UoW;
    using AdsPortal.Domain.Entities;
    using MediatR.GenericOperations.Queries;

    public class GetAdvertisementsListQuery : IGetListQuery<GetAdvertisementsListResponse>
    {
        private class Handler : GetListQueryHandler<GetAdvertisementsListQuery, Advertisement, GetAdvertisementsListResponse>
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

