﻿namespace AdsPortal.Application.Operations.CategoryOperations.Commands.UpdateCategory
{
    using System;
    using AdsPortal.Application.GenericHandlers.Relational.Commands;
    using AdsPortal.Application.Interfaces.Persistence.UoW;
    using AdsPortal.WebApi.Domain.Entities;
    using AutoMapper;
    using MediatR.GenericOperations.Commands;
    using MediatR.GenericOperations.Mapping;
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

        private class Handler : UpdateCommandHandler<UpdateCategoryCommand, UpdateCategoryValidator, Category>
        {
            public Handler(IAppRelationalUnitOfWork uow, IMapper mapper) : base(uow, mapper)
            {

            }
        }
    }
}
