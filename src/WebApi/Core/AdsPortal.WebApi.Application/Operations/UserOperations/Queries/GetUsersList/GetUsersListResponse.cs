namespace AdsPortal.Application.Operations.UserOperations.Queries.GetUsersList
{
    using System;
    using AdsPortal.WebApi.Domain.Entities;
    using AutoMapper;
    using MediatR.GenericOperations.Abstractions;
    using MediatR.GenericOperations.Mapping;

    public sealed record GetUsersListResponse : IIdentifiableOperationResult, ICustomMapping
    {
        public Guid Id { get; init; }

        public string Name { get; init; } = default!;
        public string Surname { get; init; } = default!;

        void ICustomMapping.CreateMappings(Profile configuration)
        {
            configuration.CreateMap<User, GetUsersListResponse>();
        }
    }
}
