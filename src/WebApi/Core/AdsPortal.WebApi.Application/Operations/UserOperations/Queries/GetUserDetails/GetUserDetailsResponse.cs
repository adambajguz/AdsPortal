namespace AdsPortal.WebApi.Application.Operations.UserOperations.Queries.GetUserDetails
{
    using System;
    using AdsPortal.WebApi.Domain.Entities;
    using AdsPortal.WebApi.Domain.Jwt;
    using AutoMapper;
    using MediatR.GenericOperations.Abstractions;
    using MediatR.GenericOperations.Mapping;

    public sealed record GetUserDetailsResponse : IIdentifiableOperationResult, ICustomMapping
    {
        public Guid Id { get; init; }
        public DateTime CreatedOn { get; init; }
        public Guid? CreatedBy { get; init; }
        public DateTime LastSavedOn { get; init; }
        public Guid? LastSavedBy { get; init; }

        public string Email { get; init; } = default!;

        public string Name { get; init; } = default!;
        public string Surname { get; init; } = default!;
        public string Description { get; init; } = default!;

        public Roles Role { get; init; }

        void ICustomMapping.CreateMappings(Profile configuration)
        {
            configuration.CreateMap<User, GetUserDetailsResponse>();
        }
    }
}
