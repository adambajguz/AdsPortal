namespace AdsPortal.Application.Operations.MediaItemOperations.Queries.GetMediaItemDetails
{
    using System;
    using AutoMapper;
    using AdsPortal.Application.OperationsAbstractions;
    using AdsPortal.Domain.Entities;
    using AdsPortal.Domain.Mapping;

    public class GetMediaItemFileResponse : IIdentifiableOperationResult, ICustomMapping
    {
        public Guid Id { get; set; }

        public string FileName { get; set; } = string.Empty;
        public byte[]? Data { get; set; }
        public string ContentType { get; set; } = string.Empty;
        public string Hash { get; set; } = string.Empty;

        void ICustomMapping.CreateMappings(Profile configuration)
        {
            configuration.CreateMap<MediaItem, GetMediaItemFileResponse>();
        }
    }
}
