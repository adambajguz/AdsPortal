﻿namespace AdsPortal.Application.Operations.PublicationOperations.Queries.GetPublicationsList
{
    using System.Threading;
    using System.Threading.Tasks;
    using AdsPortal.Application.GenericHandlers.Relational.Queries;
    using AdsPortal.Application.Interfaces.Persistence.UoW;
    using AdsPortal.Domain.Entities;

    public class GetPublicationsListQuery : IGetListQuery<GetPublicationsListResponse>
    {
        public GetPublicationsListQuery()
        {

        }

        private class Handler : GetListQueryHandler<GetPublicationsListQuery, Publication, GetPublicationsListResponse>
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
