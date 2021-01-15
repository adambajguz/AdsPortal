namespace AdsPortal.WebApi.Application.Operations.CategoryOperations.Commands.UpdateCategory
{
    using System;
    using AdsPortal.WebApi.Application.GenericHandlers.Relational.Commands;
    using AdsPortal.WebApi.Domain.Entities;
    using AdsPortal.WebApi.Domain.Interfaces.UoW;
    using AutoMapper;
    using AutoMapper.Extensions;
    using MediatR.GenericOperations.Commands;
    using Newtonsoft.Json;

    public sealed record UpdateCategoryCommand : IUpdateCommand
    {
        [JsonIgnore]
        public Guid Id { get; init; }

        public string? Name { get; init; } = string.Empty;
        public string? Description { get; init; } = string.Empty;

        void ICustomMapping.CreateMappings(Profile configuration)
        {
            configuration.CreateMap<UpdateCategoryCommand, Category>();
        }

        private sealed class Handler : UpdateCommandHandler<UpdateCategoryCommand, UpdateCategoryValidator, Category>
        {
            public Handler(IAppRelationalUnitOfWork uow, IMapper mapper) : base(uow, mapper)
            {

            }
        }
    }
}
