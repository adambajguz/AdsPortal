namespace AdsPortal.WebApi.Application.Operations.UserOperations.Commands.PatchUser
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using AdsPortal.WebApi.Application.GenericHandlers.Relational.Commands;
    using AdsPortal.WebApi.Application.Interfaces.Identity;
    using AdsPortal.WebApi.Domain.Entities;
    using AdsPortal.WebApi.Domain.Interfaces.UoW;
    using AutoMapper;
    using AutoMapper.Extensions;
    using FluentValidation;
    using MediatR.GenericOperations.Commands;
    using Newtonsoft.Json;

    //TODO: add/test patch user
    public sealed record PatchUserCommand : IUpdateCommand
    {
        [JsonIgnore]
        public Guid Id { get; init; }

        public PatchProperty<string?>? Email { get; init; }

        public PatchProperty<string?>? Name { get; init; }
        public PatchProperty<string?>? Surname { get; init; }

        //public PatchProperty<Roles>? Role { get; set; }

        void ICustomMapping.CreateMappings(Profile configuration)
        {
            configuration.CreateMap<PatchUserCommand, User>()
                         .ForMember(dest => dest.Email, opt =>
                             {
                                 opt.Condition((src, dest) => src.Email?.Include ?? false);
                                 opt.MapFrom(src => src.Email!.Value.Value);
                             })
                         .ForMember(dest => dest.Name, opt =>
                             {
                                 opt.Condition((src, dest) => src.Email?.Include ?? false);
                                 opt.MapFrom(src => src.Name!.Value.Value);
                             })
                         .ForMember(dest => dest.Surname, opt =>
                             {
                                 opt.Condition((src, dest) => src.Email?.Include ?? false);
                                 opt.MapFrom(src => src.Surname!.Value.Value);
                             });
            //  .ForAllMembersTryPatchProperty();
        }

        private sealed class Handler : UpdateCommandHandler<PatchUserCommand, User>
        {
            private readonly IDataRightsService _drs;

            public Handler(IAppRelationalUnitOfWork uow, IMapper mapper, IDataRightsService drs) : base(uow, mapper)
            {
                _drs = drs;
            }

            protected override async ValueTask OnValidate(User entity, CancellationToken cancellationToken)
            {
                await _drs.IsOwnerOrAdminElseThrowAsync(Command.Id);

                ////TODO: add more generic approach
                //if (data.Role.HasFlag(Roles.Admin))
                //    _drs.ValidateIsAdmin();

                //if (data.Role.HasFlag(Roles.User))
                //    _drs.HasRole(Roles.User);

                await new PatchUserValidator().ValidateAndThrowAsync(Command, cancellationToken: cancellationToken);
            }
        }
    }
}
