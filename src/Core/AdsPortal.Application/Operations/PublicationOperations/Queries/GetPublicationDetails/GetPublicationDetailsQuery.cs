namespace AdsPortal.Application.Operations.PublicationOperations.Queries.GetPublicationDetails
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using AdsPortal.Application.GenericHandlers.Relational.Queries;
    using AdsPortal.Application.Interfaces.Persistence.UoW;
    using AdsPortal.Domain.Entities;
    using AutoMapper;

    public class GetPublicationDetailsQuery : IGetDetailsByIdQuery<GetPublicationDetailsResponse>
    {
        public Guid Id { get; }

        public GetPublicationDetailsQuery(Guid id)
        {
            Id = id;
        }

        private class Handler : GetDetailsByIdQueryHandler<GetPublicationDetailsQuery, Publication, GetPublicationDetailsResponse>
        {
            public Handler(IAppRelationalUnitOfWork uow, IMapper mapper) : base(uow, mapper)
            {

            }

            protected override Task OnInit(CancellationToken cancellationToken)
            {
                return Task.CompletedTask;
            }

            protected override async Task<Publication> OnFetch(CancellationToken cancellationToken)
            {
                return await Uow.Publications.SingleById_DetailsWithRelated(Query.Id, noTracking: true, cancellationToken);
            }

            protected override Task OnMapped(Publication entity, GetPublicationDetailsResponse response, CancellationToken cancellationToken)
            {
                response.PointsToEvaluation = GetPointsToEvaluation(entity);
                response.SlotsToEvaluation = GetSlotsToEvaluation(entity);

                return Task.CompletedTask;
            }

            //TODO: move to some service or sth
            private double GetPointsToEvaluation(Publication entity)
            {
                double points = entity.Journal.Points;
                double slotsToEvaluation = GetSlots(entity);

                return Math.Round(points * slotsToEvaluation, 4);
            }

            private double GetSlots(Publication entity)
            {
                double points = entity.Journal.Points;
                int internalAuthors = entity.PublicationAuthors.Count;

                double slotsToEvalutation = 1d / internalAuthors;
                if (points < 100)
                {
                    int externalAuthors = internalAuthors + entity.ExternalAuthors;

                    slotsToEvalutation *= Math.Sqrt(internalAuthors / (double)externalAuthors);
                }

                return slotsToEvalutation;
            }

            private double GetSlotsToEvaluation(Publication entity)
            {
                return Math.Round(GetSlots(entity), 2);
            }
        }
    }
}
