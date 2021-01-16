namespace AdsPortal.WebApi.Application.Operations.AdvertisementOperations.Commands.CreateAdvertisement
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

    public sealed record CreateAdvertisementCommand : ICreateCommand
    {
        public string? Title { get; init; } = string.Empty;
        public string? Description { get; init; } = string.Empty;
        public bool IsPublished { get; init; }
        public DateTime VisibleTo { get; init; }

        public Guid? CoverImageId { get; init; }
        public Guid CategoryId { get; init; }
        public Guid AuthorId { get; init; }

        void ICustomMapping.CreateMappings(Profile configuration)
        {
            configuration.CreateMap<CreateAdvertisementCommand, Advertisement>();
        }

        private sealed class Handler : CreateCommandHandler<CreateAdvertisementCommand, Advertisement>
        {
            private readonly IJobSchedulingService _jobScheduling;
            private readonly IDataRightsService _drs;

            public Handler(IJobSchedulingService jobScheduling, IDataRightsService drs, IAppRelationalUnitOfWork uow, IMapper mapper) : base(uow, mapper)
            {
                _jobScheduling = jobScheduling;
                _drs = drs;
            }

            protected override async ValueTask OnValidate(CancellationToken cancellationToken)
            {
                await ValidationUtils.ValidateAndThrowAsync<CreateAdvertisementValidator, CreateAdvertisementCommand>(Command, cancellationToken);

                await _drs.IsOwnerOrAdminElseThrowAsync(Command.AuthorId);

                await Uow.Categories.ExistsByIdElseThrowAsync(Command.CategoryId);

                if (Command.CoverImageId is Guid coverId)
                {
                    await Uow.MediaItems.ExistsByIdElseThrowAsync(coverId);
                }
            }

            protected override async ValueTask OnAdded(Advertisement entity, CancellationToken cancellationToken)
            {
                if (entity.IsPublished)
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
