namespace AdsPortal.Application.Operations.MediaItemOperations.Queries.GetMediaItemsStatistics
{
    using AdsPortal.WebApi.Domain.Models;
    using AutoMapper;
    using MediatR.GenericOperations.Abstractions;
    using MediatR.GenericOperations.Mapping;

    public sealed record GetMediaItemsStatisticsResponse : IOperationResult, ICustomMapping
    {
        public int Count { get; init; }
        public long TotalByteSize { get; init; }

        public long MinByteSize { get; init; }
        public double AverageByteSize { get; init; }
        public long MaxByteSize { get; init; }

        void ICustomMapping.CreateMappings(Profile configuration)
        {
            configuration.CreateMap<StatisticsModel<long>, GetMediaItemsStatisticsResponse>()
                         .ForMember(dest => dest.TotalByteSize, cfg => cfg.MapFrom(src => src.Sum))
                         .ForMember(dest => dest.MinByteSize, cfg => cfg.MapFrom(src => src.Min))
                         .ForMember(dest => dest.AverageByteSize, cfg => cfg.MapFrom(src => src.Average))
                         .ForMember(dest => dest.MaxByteSize, cfg => cfg.MapFrom(src => src.Max));
        }
    }
}
