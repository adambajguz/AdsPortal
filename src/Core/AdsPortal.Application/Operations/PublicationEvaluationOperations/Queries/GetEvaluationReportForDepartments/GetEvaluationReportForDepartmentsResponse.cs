namespace AdsPortal.Application.Operations.PublicationEvaluationOperations.Queries.GetEvaluationReportForDepartments
{
    using System;
    using System.Collections.Generic;
    using AdsPortal.Application.Operations.PublicationEvaluationOperations.Queries.GetEvaluationReportForAuthors;
    using AdsPortal.Application.OperationsAbstractions;
    using AdsPortal.Domain.Entities;
    using AdsPortal.Domain.Mapping;
    using AutoMapper;

    public class GetEvaluationReportForDepartmentsResponse : IOperationResult
    {
        public DateTime CreatedOn { get; }

        public ICollection<EvaluationReportForDepartment> Report { get; }

        public GetEvaluationReportForDepartmentsResponse(ICollection<EvaluationReportForDepartment> report)
        {
            CreatedOn = DateTime.UtcNow;
            Report = report;
        }
    }

    public class EvaluationReportForDepartment : IOperationResult, ICustomMapping
    {
        public Guid DepartmentId { get; set; }
        public string Name { get; set; } = string.Empty;

        public int PublicationsCount { get; set; }

        public double PointsToEvaluation { get; set; }
        public double SlotsToEvaluation { get; set; }

        public ICollection<EvaluationReportForAuthor> AuthorReport { get; set; } = new List<EvaluationReportForAuthor>();

        void ICustomMapping.CreateMappings(Profile configuration)
        {
            configuration.CreateMap<Department, EvaluationReportForDepartment>()
                         .ForMember(dest => dest.DepartmentId, cfg => cfg.MapFrom(src => src.Id));
        }
    }
}
