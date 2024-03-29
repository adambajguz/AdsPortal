﻿namespace AdsPortal.WebApi.Application.Operations.MediaItemOperations.Queries.GetMediaItemsList
{
    using System;
    using AdsPortal.WebApi.Domain.Entities;
    using AdsPortal.WebApi.Domain.Jwt;
    using AutoMapper;
    using AutoMapper.Extensions;
    using MediatR.GenericOperations.Abstractions;

    public sealed record GetMediaItemsListResponse : IIdentifiableOperationResult, ICustomMapping
    {
        public Guid Id { get; init; }

        public string FileName { get; init; } = string.Empty;
        public string Description { get; init; } = string.Empty;
        public string Alt { get; init; } = string.Empty;

        public string VirtualDirectory { get; init; } = string.Empty;
        public string ContentType { get; init; } = string.Empty;
        public long ByteSize { get; init; }

        public Guid? OwnerId { get; init; }
        public Roles Role { get; init; }

        void ICustomMapping.CreateMappings(Profile configuration)
        {
            configuration.CreateMap<MediaItem, GetMediaItemsListResponse>();
        }
    }
}
