namespace AdsPortal.Application.Operations.DepartmentOperations.Commands.CreateDepartment
{
    using System.Threading;
    using System.Threading.Tasks;
    using AutoMapper;
    using AdsPortal.Application.GenericHandlers.Relational.Commands;
    using AdsPortal.Application.Interfaces.Persistence.UoW;
    using AdsPortal.Domain.Entities;
    using AdsPortal.Domain.Mapping;

    public class CreateDepartmentCommand : ICreateCommand
    {
        public string? Name { get; set; } = string.Empty;

        void ICustomMapping.CreateMappings(Profile configuration)
        {
            configuration.CreateMap<CreateDepartmentCommand, Department>();
        }

        private class Handler : CreateCommandHandler<CreateDepartmentCommand, CreateDepartmentValidator, Department>
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
