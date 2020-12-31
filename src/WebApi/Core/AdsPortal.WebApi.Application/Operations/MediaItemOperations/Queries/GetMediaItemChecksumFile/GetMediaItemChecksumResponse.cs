namespace AdsPortal.Application.Operations.MediaItemOperations.Queries.GetMediaItemDetails
{
    using System;
    using System.Text;
    using AdsPortal.WebApi.Domain.Entities;
    using AutoMapper;
    using MediatR.GenericOperations.Abstractions;
    using MediatR.GenericOperations.Mapping;

    public class GetMediaItemChecksumResponse : IIdentifiableOperationResult, ICustomMapping
    {
        public Guid Id { get; init; }

        //TODO: remove setter; add init
        public string FileName { get; set; } = string.Empty;
        public string ContentType { get; set; } = string.Empty;
        public string Hash { get; init; } = string.Empty;

        public string FileContent { get; set; } = string.Empty;
        public byte[] FileByteContent => Encoding.UTF8.GetBytes(FileContent);

        void ICustomMapping.CreateMappings(Profile configuration)
        {
            configuration.CreateMap<MediaItem, GetMediaItemChecksumResponse>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.FileName, opt => opt.MapFrom(src => src.FileName))
                .ForMember(dest => dest.Hash, opt => opt.MapFrom(src => src.Hash))
                .ForAllOtherMembers(opt => opt.Ignore());
        }
    }
}
