namespace AdsPortal.Application.Operations.MediaItemOperations.Queries.GetMediaItemDetails
{
    using System;
    using System.Text;
    using AutoMapper;
    using AdsPortal.Application.OperationsAbstractions;
    using AdsPortal.Domain.Entities;
    using AdsPortal.Domain.Mapping;

    public class GetMediaItemChecksumResponse : IIdentifiableOperationResult, ICustomMapping
    {
        public Guid Id { get; set; }

        public string FileName { get; set; } = string.Empty;
        public string ContentType { get; set; } = string.Empty;
        public string Hash { get; set; } = string.Empty;

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
