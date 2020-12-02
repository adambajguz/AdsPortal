namespace AdsPortal.Application.Operations.MediaItemOperations.Queries.GetMediaItemsStatistics
{
    using AutoMapper;
    using AdsPortal.Application.OperationsAbstractions;
    using AdsPortal.Domain.Mapping;
    using AdsPortal.Domain.Models;

    public class GetMediaItemsStatisticsResponse : IOperationResult, ICustomMapping
    {
        public int Count { get; set; }
        public long TotalByteSize { get; set; }

        public long MinByteSize { get; set; }
        public double AverageByteSize { get; set; }
        public long MaxByteSize { get; set; }

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
