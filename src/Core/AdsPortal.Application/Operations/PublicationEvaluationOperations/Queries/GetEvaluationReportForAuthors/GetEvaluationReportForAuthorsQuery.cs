namespace AdsPortal.Application.Operations.PublicationEvaluationOperations.Queries.GetEvaluationReportForAuthors
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using AutoMapper;
    using AdsPortal.Application.Interfaces.Persistence.UoW;
    using AdsPortal.Application.OperationsAbstractions;
    using AdsPortal.Domain.Entities;
    using MediatR;

    public class GetEvaluationReportForAuthorsQuery : IOperation<GetEvaluationReportForAuthorsResponse>
    {
        public GetEvaluationReportForAuthorsQuery()
        {

        }

        private class Handler : IRequestHandler<GetEvaluationReportForAuthorsQuery, GetEvaluationReportForAuthorsResponse>
        {
            private readonly IAppRelationalUnitOfWork _uow;
            private readonly IMapper _mapper;

            public Handler(IAppRelationalUnitOfWork uow, IMapper mapper)
            {
                _uow = uow;
                _mapper = mapper;
            }

            public async Task<GetEvaluationReportForAuthorsResponse> Handle(GetEvaluationReportForAuthorsQuery request, CancellationToken cancellationToken)
            {
                List<EvaluationReportForAuthor> report = new List<EvaluationReportForAuthor>();

                // TODO: This has high ram usage because a lot of authors or publications need to be loaded
                List<Author> authors = await _uow.Authors.AllAsync_WithRelated(noTracking: true, cancellationToken: cancellationToken);

                foreach (Author author in authors)
                {
                    int publicationsCount = author.PublicationAuthors.Count;

                    IEnumerable<double> points = author.PublicationAuthors.Select(x => GetPointsToEvaluation(x.Publication));
                    IEnumerable<double> slots = author.PublicationAuthors.Select(x => GetSlotsToEvaluation(x.Publication));

                    EvaluationReportForAuthor response = _mapper.Map<EvaluationReportForAuthor>(author); //TODO: Consider using ProjectTo in repository instead of Map
                    response.PublicationsCount = publicationsCount;

                    if (publicationsCount > 0)
                    {
                        response.WorstPublicationPoints = points.Min();
                        response.AveragePublicationPoints = points.Average();
                        response.BestPublicationPoints = points.Max();

                        response.PointsToEvaluation = points.Sum();
                        response.SlotsToEvaluation = slots.Sum();
                    }

                    Dictionary<double, int> groupedPublicationPoints = author.PublicationAuthors.GroupBy(x => x.Publication.Journal.Points)
                                                                                                .ToDictionary(x => x.Key, x => x.Count());

                    response.GroupedPublicationPoints = groupedPublicationPoints;

                    report.Add(response);
                }

                return new GetEvaluationReportForAuthorsResponse(report.OrderByDescending(x => x.PointsToEvaluation).ToList());
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
