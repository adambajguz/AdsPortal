namespace AdsPortal.Application.Operations.MediaItemOperations.Queries.GetMediaItemsStatistics
{
    using System.Threading;
    using System.Threading.Tasks;
    using AdsPortal.Application.Interfaces.Persistence.UoW;
    using AdsPortal.Application.OperationsAbstractions;
    using AdsPortal.Domain.Models;
    using AutoMapper;
    using MediatR;

    public class GetMediaItemsStatisticsQuery : IOperation<GetMediaItemsStatisticsResponse>
    {
        private class Handler : IRequestHandler<GetMediaItemsStatisticsQuery, GetMediaItemsStatisticsResponse>
        {
            private readonly IAppRelationalUnitOfWork _uow;
            private readonly IMapper _mapper;

            public Handler(IAppRelationalUnitOfWork uow, IMapper mapper)
            {
                _uow = uow;
                _mapper = mapper;
            }

            public async Task<GetMediaItemsStatisticsResponse> Handle(GetMediaItemsStatisticsQuery command, CancellationToken cancellationToken)
            {
                StatisticsModel<long> statistics = await _uow.MediaItems.GetByteSizeStatisticsAsync();
                GetMediaItemsStatisticsResponse response = _mapper.Map<GetMediaItemsStatisticsResponse>(statistics);

                return response;
            }
        }
    }
}
