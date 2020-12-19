namespace AdsPortal.Application.Operations.AnalyticsRecordOperations.Commands.CreateOrUpdateAnalyticsRecord
{
    using System;
    using System.Data.HashFunction;
    using System.Data.HashFunction.MurmurHash;
    using System.Threading;
    using System.Threading.Tasks;
    using AdsPortal.Application.Interfaces.Persistence.UoW;
    using AdsPortal.Domain.Entities;
    using AutoMapper;
    using MediatR;
    using MediatR.GenericOperations.Abstractions;
    using MediatR.GenericOperations.Mapping;
    using MediatR.GenericOperations.Models;

    public class CreateOrUpdateAnalyticsRecordCommand : IOperation<IdResult>, ICustomMapping
    {
        public string? Uri { get; init; }
        public string? UserAgent { get; init; }
        public string? Ip { get; init; }

        void ICustomMapping.CreateMappings(Profile configuration)
        {
            configuration.CreateMap<CreateOrUpdateAnalyticsRecordCommand, AnalyticsRecord>();
        }

        private class Handler : IRequestHandler<CreateOrUpdateAnalyticsRecordCommand, IdResult>
        {
            private static readonly IMurmurHash2 _hasher = MurmurHash2Factory.Instance.Create(new MurmurHash2Config
            {
                HashSizeInBits = 64,
                Seed = 46789130U,
            });

            private readonly IMongoUnitOfWork _uow;
            private readonly IMapper _mapper;

            public Handler(IMongoUnitOfWork uow, IMapper mapper)
            {
                _uow = uow;
                _mapper = mapper;
            }

            public async Task<IdResult> Handle(CreateOrUpdateAnalyticsRecordCommand command, CancellationToken cancellationToken)
            {
                DateTime now = DateTime.UtcNow.Date;

                IHashValue? hashValue = _hasher.ComputeHash(now.ToString() + command.Uri ?? string.Empty + command.UserAgent ?? string.Empty);
                long hash = BitConverter.ToInt64(hashValue.Hash);

                AnalyticsRecord? prevEntity = await _uow.AnalyticsRecords.SingleOrDefaultAsync(x => x.Hash == hash && x.Timestamp == now && x.Uri == command.Uri && x.UserAgent == command.UserAgent);

                if (prevEntity is null)
                {
                    AnalyticsRecord entity = _mapper.Map<AnalyticsRecord>(command);
                    entity.Hash = hash;
                    entity.Timestamp = now;
                    entity.Visits = 1;

                    await _uow.AnalyticsRecords.AddAsync(entity);

                    return new IdResult { Id = entity.Id };
                }

                ++prevEntity.Visits;
                await _uow.AnalyticsRecords.UpdateAsync(prevEntity);

                return new IdResult { Id = prevEntity.Id };
            }
        }
    }
}
