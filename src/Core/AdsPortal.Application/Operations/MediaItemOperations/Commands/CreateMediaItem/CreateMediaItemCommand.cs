namespace AdsPortal.Application.Operations.MediaItemOperations.Commands.CreateMediaItem
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using AdsPortal.Application.Extensions;
    using AdsPortal.Application.GenericHandlers.Relational.Commands;
    using AdsPortal.Application.Interfaces.Persistence.UoW;
    using AdsPortal.Common.Utils;
    using AdsPortal.Domain.Entities;
    using AdsPortal.Domain.Jwt;
    using AdsPortal.Domain.Mapping;
    using AdsPortal.Domain.Utils;
    using AutoMapper;
    using Microsoft.AspNetCore.Http;

    public class CreateMediaItemCommand : ICreateCommand
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

        private class FileNameResolver : IValueResolver<CreateMediaItemCommand, MediaItem, string>
        {
            public string Resolve(CreateMediaItemCommand source, MediaItem destination, string member, ResolutionContext context)
            {
                if (source.GenerateFileName)
                    return FileUtils.GenerateEmptyFileName();

                bool v = string.IsNullOrWhiteSpace(source.NewFileName);
                if (v && source.File != null)
                    return source.File.FileName;

                if (!v)
                    return source.NewFileName!.Trim();

                return FileUtils.GenerateEmptyFileName();
            }
        }

        private class Handler : CreateCommandHandler<CreateMediaItemCommand, CreateMediaItemValidator, MediaItem>
        {
            public Handler(IAppRelationalUnitOfWork uow, IMapper mapper) : base(uow, mapper)
            {

            }

            protected override Task OnInit(CancellationToken cancellationToken)
            {
                return Task.CompletedTask;
            }

            protected override async Task OnMapped(MediaItem entity, CancellationToken cancellationToken)
            {
                entity.Data = await Command.File.GetBytesAsync();
                entity.Hash = HashingUtils.GetSHA512Hex(entity.Data);
                entity.PathHashCode = entity.CalculatePathHashCode();
            }
        }
    }
}
