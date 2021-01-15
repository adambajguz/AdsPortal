namespace AdsPortal.WebApi.Application.Operations.MediaItemOperations.Commands.CreateMediaItem
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using AdsPortal.Shared.Extensions.Utils;
    using AdsPortal.WebApi.Application.Extensions;
    using AdsPortal.WebApi.Application.GenericHandlers.Relational.Commands;
    using AdsPortal.WebApi.Application.Interfaces.Persistence.UoW;
    using AdsPortal.WebApi.Application.Utils;
    using AdsPortal.WebApi.Domain.Entities;
    using AdsPortal.WebApi.Domain.Jwt;
    using AdsPortal.WebApi.Domain.Utils;
    using AutoMapper;
    using AutoMapper.Extensions;
    using MediatR.GenericOperations.Commands;
    using Microsoft.AspNetCore.Http;

    public sealed record CreateMediaItemCommand : ICreateCommand
    {
        public IFormFile? File { get; init; }

        public string? NewFileName { get; init; } = string.Empty;
        public bool GenerateFileName { get; init; }

        public string Description { get; init; } = string.Empty;
        public string Alt { get; init; } = string.Empty;

        public string VirtualDirectory { get; init; } = string.Empty;

        public Guid? OwnerId { get; init; }
        public Roles Role { get; init; }

        void ICustomMapping.CreateMappings(Profile configuration)
        {
            configuration.CreateMap<CreateMediaItemCommand, MediaItem>()
                         .ForMember(dest => dest.FileName, opt => opt.MapFrom(new FileNameResolver()))
                         .ForMember(dest => dest.ContentType, opt => opt.MapFrom(src => src.File == null ? string.Empty : src.File.ContentType))
                         .ForMember(dest => dest.ByteSize, opt => opt.MapFrom(src => src.File == null ? 0 : src.File.Length));
        }

        private sealed class FileNameResolver : IValueResolver<CreateMediaItemCommand, MediaItem, string>
        {
            public string Resolve(CreateMediaItemCommand source, MediaItem destination, string member, ResolutionContext context)
            {
                if (source.GenerateFileName)
                {
                    return FileUtils.GenerateEmptyFileName();
                }

                bool v = string.IsNullOrWhiteSpace(source.NewFileName);
                if (v && source.File != null)
                {
                    return source.File.FileName;
                }

                if (!v)
                {
                    return source.NewFileName!.Trim();
                }

                return FileUtils.GenerateEmptyFileName();
            }
        }

        private sealed class Handler : CreateCommandHandler<CreateMediaItemCommand, CreateMediaItemValidator, MediaItem>
        {
            public Handler(IAppRelationalUnitOfWork uow, IMapper mapper) : base(uow, mapper)
            {

            }

            protected override async ValueTask<MediaItem> OnMapped(MediaItem entity, CancellationToken cancellationToken)
            {
                entity.Data = await Command.File.GetBytesAsync();
                entity.Hash = HashingUtils.GetSHA512Hex(entity.Data ?? Array.Empty<byte>());
                entity.PathHashCode = entity.CalculatePathHash();

                return entity;
            }
        }
    }
}
