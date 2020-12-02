namespace AdsPortal.Application.Operations.UserOperations.Queries.GetUsersList
{
    using System;
    using AutoMapper;
    using AdsPortal.Application.OperationsAbstractions;
    using AdsPortal.Domain.Entities;
    using AdsPortal.Domain.Mapping;


    public class GetUsersListResponse : IIdentifiableOperationResult, ICustomMapping
    {
        public Guid Id { get; set; }

        public string Email { get; set; } = default!;

        void ICustomMapping.CreateMappings(Profile configuration)
        {
            configuration.CreateMap<User, GetUsersListResponse>();
        }
    }
}
