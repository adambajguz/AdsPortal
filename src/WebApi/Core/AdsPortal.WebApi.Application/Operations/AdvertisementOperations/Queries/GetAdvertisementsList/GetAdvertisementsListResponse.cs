﻿namespace AdsPortal.WebApi.Application.Operations.AdvertisementOperations.Queries.GetAdvertisementsList
{
    using System;
    using AdsPortal.WebApi.Application.Models;
    using AdsPortal.WebApi.Domain.Entities;
    using AutoMapper;
    using AutoMapper.Extensions;
    using MediatR.GenericOperations.Abstractions;

    public sealed record GetAdvertisementsListResponse : IIdentifiableOperationResult, ICustomMapping
    {
        public Guid Id { get; init; }

        public string Title { get; init; } = string.Empty;
        public string Description { get; init; } = string.Empty;

        public bool IsPublished { get; init; }
        public DateTime VisibleTo { get; init; }

        public MediaFileResponse? CoverImage { get; init; }

        public Guid CategoryId { get; init; }
        public Guid AuthorId { get; init; }

        void ICustomMapping.CreateMappings(Profile configuration)
        {
            configuration.CreateMap<Advertisement, GetAdvertisementsListResponse>();
        }
    }
}
