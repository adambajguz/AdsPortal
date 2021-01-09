namespace AdsPortal.WebApi.Application.Operations.UserOperations.Commands.PatchUser
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using AdsPortal.WebApi.Application.GenericHandlers.Relational.Commands;
    using AdsPortal.WebApi.Application.Interfaces.Identity;
    using AdsPortal.WebApi.Application.Interfaces.Persistence.UoW;
    using AdsPortal.WebApi.Domain.Entities;
    using AutoMapper;
    using FluentValidation;
    using MediatR.GenericOperations.Commands;
    using MediatR.GenericOperations.Mapping;
    using MediatR.GenericOperations.Models;
    using Newtonsoft.Json;

    //TODO: add/test patch user
    public sealed record PatchUserCommand : IUpdateCommand
    {
        [JsonIgnore]
        public Guid Id { get; init; }

        public PatchProperty<string?>? Email { get; init; }

        public PatchProperty<string?>? Name { get; init; }
        public PatchProperty<string?>? Surname { get; init; }
        public PatchProperty<string?>? Description { get; init; }

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
                             })
                         .ForMember(dest => dest.Description, opt =>
                             {
                                 opt.Condition((src, dest) => src.Email?.Include ?? false);
                                 opt.MapFrom(src => src.Description!.Value.Value);
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
                await _drs.IsOwnerOrAdminElseThrow(Command.Id);

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
