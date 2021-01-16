namespace AdsPortal.WebApi.Application.Operations.AdvertisementOperations.Commands.UpdateAdvertisement
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using AdsPortal.WebApi.Application.GenericHandlers.Relational.Commands;
    using AdsPortal.WebApi.Application.Interfaces.Identity;
    using AdsPortal.WebApi.Application.Interfaces.JobScheduler;
    using AdsPortal.WebApi.Application.Jobs;
    using AdsPortal.WebApi.Application.Utils;
    using AdsPortal.WebApi.Domain.EmailTemplates;
    using AdsPortal.WebApi.Domain.Entities;
    using AdsPortal.WebApi.Domain.Interfaces.UoW;
    using AutoMapper;
    using AutoMapper.Extensions;
    using MediatR.GenericOperations.Commands;
    using Newtonsoft.Json;

    public sealed record UpdateAdvertisementCommand : IUpdateCommand
    {
        [JsonIgnore]
        public Guid Id { get; init; }

        public string? Title { get; init; } = string.Empty;
        public string? Description { get; init; } = string.Empty;
        public bool IsPublished { get; init; }
        public DateTime VisibleTo { get; init; }

        public Guid? CoverImageId { get; init; }
        public Guid CategoryId { get; init; }
        public Guid AuthorId { get; init; }

        void ICustomMapping.CreateMappings(Profile configuration)
        {
            configuration.CreateMap<UpdateAdvertisementCommand, Advertisement>();
        }

        private sealed class Handler : UpdateCommandHandler<UpdateAdvertisementCommand, Advertisement>
        {
            private readonly IJobSchedulingService _jobScheduling;
            private readonly IDataRightsService _drs;

            private bool WillBePublished { get; set; }

            public Handler(IJobSchedulingService jobScheduling, IDataRightsService drs, IAppRelationalUnitOfWork uow, IMapper mapper) : base(uow, mapper)
            {
                _jobScheduling = jobScheduling;
                _drs = drs;
            }

            protected override async ValueTask OnValidate(Advertisement advertisement, CancellationToken cancellationToken)
            {
                await ValidationUtils.ValidateAndThrowAsync<UpdateAdvertisementValidator, UpdateAdvertisementCommand>(Command, cancellationToken);

                await _drs.IsOwnerOrAdminElseThrowAsync(Command.AuthorId);

                await Uow.Categories.ExistsByIdElseThrowAsync(Command.CategoryId);

                if (Command.CoverImageId is Guid coverId)
                {
                    await Uow.MediaItems.ExistsByIdElseThrowAsync(coverId);
                }

                if (advertisement.IsPublished != Command.IsPublished && Command.IsPublished)
                    WillBePublished = true; //TODO: add OnBeforeMapped
            }

            protected override async ValueTask OnUpdated(Advertisement entity, CancellationToken cancellationToken)
            {
                if (WillBePublished)
                {
                    SendEmailJobArguments args = new()
                    {
                        Email = _drs.CurrentUser.Email,
                        Template = new AdvertisementPublishedEmail { UserName = _drs.CurrentUser.Name, AdvertisementTitle = entity.Title, AdvertisementVisibleTo = entity.VisibleTo }
                    };

                    await _jobScheduling.ScheduleAsync<SendEmailJob>(operationArguments: args, cancellationToken: cancellationToken);
                }
            }
        }
    }
}
