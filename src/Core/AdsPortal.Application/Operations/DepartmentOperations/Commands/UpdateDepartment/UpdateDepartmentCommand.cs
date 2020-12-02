namespace AdsPortal.Application.Operations.DepartmentOperations.Commands.UpdateDepartment
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using AdsPortal.Application.GenericHandlers.Relational.Commands;
    using AdsPortal.Application.Interfaces.Persistence.UoW;
    using AdsPortal.Domain.Entities;
    using AdsPortal.Domain.Mapping;
    using AutoMapper;

    public class UpdateDepartmentCommand : IUpdateCommand
    {
        public Guid Id { get; set; }

        public string? Name { get; set; } = string.Empty;

        void ICustomMapping.CreateMappings(Profile configuration)
        {
            configuration.CreateMap<UpdateDepartmentCommand, Department>();
        }

        private class Handler : UpdateCommandHandler<UpdateDepartmentCommand, UpdateDepartmentValidator, Department>
        {
            public Handler(IAppRelationalUnitOfWork uow, IMapper mapper) : base(uow, mapper)
            {

            }

            protected override Task OnInit(CancellationToken cancellationToken)
            {
                return Task.CompletedTask;
            }
        }
    }
}
