namespace AdsPortal.Application.Operations.UserOperations.Commands.PatchUser
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using AutoMapper;
    using AdsPortal.Application.GenericHandlers.Relational.Commands;
    using AdsPortal.Application.Interfaces.Identity;
    using AdsPortal.Application.Interfaces.Persistence.UoW;
    using AdsPortal.Application.OperationsModels.Core;
    using AdsPortal.Domain.Entities;
    using AdsPortal.Domain.Mapping;
    using FluentValidation;

    public class PatchUserCommand : IUpdateCommand
    {
        public Guid Id { get; set; }

        public PatchProperty<string?>? Email { get; set; }

        public PatchProperty<string?>? Name { get; set; }
        public PatchProperty<string?>? Surname { get; set; }
        public PatchProperty<string?>? PhoneNumber { get; set; }
        public PatchProperty<string?>? Address { get; set; }

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
                         .ForMember(dest => dest.PhoneNumber, opt =>
                             {
                                 opt.Condition((src, dest) => src.Email?.Include ?? false);
                                 opt.MapFrom(src => src.PhoneNumber!.Value.Value);
                             })
                         .ForMember(dest => dest.Address, opt =>
                             {
                                 opt.Condition((src, dest) => src.Email?.Include ?? false);
                                 opt.MapFrom(src => src.Address!.Value.Value);
                             });
            //  .ForAllMembersTryPatchProperty();
        }

        private class Handler : UpdateCommandHandler<PatchUserCommand, User>
        {
            private readonly IDataRightsService _drs;

            public Handler(IAppRelationalUnitOfWork uow, IMapper mapper, IDataRightsService drs) : base(uow, mapper)
            {
                _drs = drs;
            }

            protected override Task OnInit(CancellationToken cancellationToken)
            {
                return Task.CompletedTask;
            }

            protected override async Task OnValidate(User entity, CancellationToken cancellationToken)
            {
                await _drs.IsOwnerOrAdminElseThrow(Command.Id);

                ////TODO: add more generic approach
                //if (data.Role.HasFlag(Roles.Admin))
                //    _drs.ValidateIsAdmin();

                //if (data.Role.HasFlag(Roles.Editor))
                //    _drs.HasRole(Roles.Editor);

                //if (data.Role.HasFlag(Roles.User))
                //    _drs.HasRole(Roles.User);

                await new PatchUserValidator().ValidateAndThrowAsync(Command, cancellationToken: cancellationToken);
            }
        }
    }
}
