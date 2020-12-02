namespace AdsPortal.Application.Operations.PublicationEvaluationOperations.Queries.GetEvaluationReportForDepartments
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using AdsPortal.Application.Interfaces.Persistence.UoW;
    using AdsPortal.Application.Operations.PublicationEvaluationOperations.Queries.GetEvaluationReportForAuthors;
    using AdsPortal.Application.OperationsAbstractions;
    using AdsPortal.Domain.Abstractions.Base;
    using AdsPortal.Domain.Entities;
    using AutoMapper;
    using MediatR;

    public class GetEvaluationReportForDepartmentsQuery : IOperation<GetEvaluationReportForDepartmentsResponse>
    {
        public bool IncludeAuthorReports { get; set; }

        public GetEvaluationReportForDepartmentsQuery()
        {

        }

        private class Handler : IRequestHandler<GetEvaluationReportForDepartmentsQuery, GetEvaluationReportForDepartmentsResponse>
        {
            private readonly IAppRelationalUnitOfWork _uow;
            private readonly IMapper _mapper;

            public Handler(IAppRelationalUnitOfWork uow, IMapper mapper)
            {
                _uow = uow;
                _mapper = mapper;
            }

            public async Task<GetEvaluationReportForDepartmentsResponse> Handle(GetEvaluationReportForDepartmentsQuery query, CancellationToken cancellationToken)
            {
                List<EvaluationReportForDepartment> report = new List<EvaluationReportForDepartment>();

                // TODO: refactor: get departments then go crawl tables
                // TODO: This has high ram usage because a lot of authors or publications need to be loaded
                List<Author> authors = await _uow.Authors.AllAsync_WithRelated(noTracking: true, cancellationToken: cancellationToken);

                IEnumerable<IGrouping<Department, Author>> groupedAuthors = authors.GroupBy(x => x.Department, new EntityIdEqualityComparer<Department>());

                foreach (IGrouping<Department, Author> department in groupedAuthors)
                {
                    EvaluationReportForDepartment response = _mapper.Map<EvaluationReportForDepartment>(department.Key); //TODO: Consider using ProjectTo in repository instead of Map

                    //TODO: refactor to remove code dubplicates between different reports
                    foreach (Author author in department)
                    {
                        int publicationsCount = author.PublicationAuthors.Count;

                        EvaluationReportForAuthor authorResponse = _mapper.Map<EvaluationReportForAuthor>(author); //TODO: Consider using ProjectTo in repository instead of Map
                        authorResponse.PublicationsCount = publicationsCount;

                        IEnumerable<double> points = author.PublicationAuthors.Select(x => GetPointsToEvaluation(x.Publication));
                        IEnumerable<double> slots = author.PublicationAuthors.Select(x => GetSlotsToEvaluation(x.Publication));

                        response.PublicationsCount += publicationsCount;

                        if (publicationsCount > 0)
                        {
                            authorResponse.WorstPublicationPoints = points.Min();
                            authorResponse.AveragePublicationPoints = points.Average();
                            authorResponse.BestPublicationPoints = points.Max();

                            authorResponse.PointsToEvaluation = points.Sum();
                            authorResponse.SlotsToEvaluation = slots.Sum();

                            response.PointsToEvaluation += authorResponse.PointsToEvaluation;
                            response.SlotsToEvaluation += authorResponse.SlotsToEvaluation;
                        }

                        if (query.IncludeAuthorReports)
                        {
                            Dictionary<double, int> groupedPublicationPoints = author.PublicationAuthors.GroupBy(x => x.Publication.Journal.Points)
                                                                                                        .ToDictionary(x => x.Key, x => x.Count());

                            authorResponse.GroupedPublicationPoints = groupedPublicationPoints;

                            response.AuthorReport.Add(authorResponse);
                        }
                    }

                    if (query.IncludeAuthorReports)
                        response.AuthorReport = response.AuthorReport.OrderByDescending(x => x.PointsToEvaluation).ToList();

                    report.Add(response);
                }

                return new GetEvaluationReportForDepartmentsResponse(report.OrderByDescending(x => x.PointsToEvaluation).ToList());
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

    internal class EntityIdEqualityComparer<TEntity> : IEqualityComparer<TEntity>
        where TEntity : class, IBaseIdentifiableEntity
    {
        public bool Equals(TEntity? e1, TEntity? e2)
        {
            return !(e1 is null || e2 is null) && e1.Id == e2.Id;
        }

        public int GetHashCode(TEntity entity)
        {
            return entity.Id.GetHashCode();
        }
    }
}
