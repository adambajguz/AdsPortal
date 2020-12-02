namespace AdsPortal.Application.Operations.PublicationEvaluationOperations.Queries.GetEvaluationReportForAuthors
{
    using System;
    using System.Collections.Generic;
    using AdsPortal.Application.OperationsAbstractions;
    using AdsPortal.Domain.Entities;
    using AdsPortal.Domain.Mapping;
    using AutoMapper;

    public class GetEvaluationReportForAuthorsResponse : IOperationResult
    {
        public DateTime CreatedOn { get; }

        public ICollection<EvaluationReportForAuthor> Report { get; }

        public GetEvaluationReportForAuthorsResponse(ICollection<EvaluationReportForAuthor> report)
        {
            CreatedOn = DateTime.UtcNow;
            Report = report;
        }
    }

    public class EvaluationReportForAuthor : IOperationResult, ICustomMapping
    {
        public Guid AuthorId { get; set; }

        public string Name { get; set; } = string.Empty;
        public string Surname { get; set; } = string.Empty;

        public Guid DepartmentId { get; set; }
        public string Department { get; set; } = string.Empty;

        public int PublicationsCount { get; set; }

        public double WorstPublicationPoints { get; set; }
        public double AveragePublicationPoints { get; set; }
        public double BestPublicationPoints { get; set; }

        public double PointsToEvaluation { get; set; }
        public double SlotsToEvaluation { get; set; }

        public Dictionary<double, int> GroupedPublicationPoints { get; set; } = new Dictionary<double, int>();

        void ICustomMapping.CreateMappings(Profile configuration)
        {
            configuration.CreateMap<Author, EvaluationReportForAuthor>()
                         .ForMember(dest => dest.AuthorId, cfg => cfg.MapFrom(src => src.Id))
                         .ForMember(dest => dest.Department, cfg => cfg.MapFrom(src => src.Department.Name));
        }
    }
}
