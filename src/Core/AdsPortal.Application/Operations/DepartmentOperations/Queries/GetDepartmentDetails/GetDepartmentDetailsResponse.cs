namespace AdsPortal.Application.Operations.DepartmentOperations.Queries.GetDepartmentDetails
{
    using System;
    using AdsPortal.Application.OperationsAbstractions;
    using AdsPortal.Domain.Mapping;
    using AutoMapper;
    using Domain.Entities;

    public class GetDepartmentDetailsResponse : IIdentifiableOperationResult, ICustomMapping
    {
        public Guid Id { get; set; }

        public DateTime CreatedOn { get; set; }
        public Guid? CreatedBy { get; set; }
        public DateTime LastSavedOn { get; set; }
        public Guid? LastSavedBy { get; set; }

        public string Name { get; set; } = string.Empty;
        void ICustomMapping.CreateMappings(Profile configuration)
        {
            configuration.CreateMap<Department, GetDepartmentDetailsResponse>();
        }
    }
}
