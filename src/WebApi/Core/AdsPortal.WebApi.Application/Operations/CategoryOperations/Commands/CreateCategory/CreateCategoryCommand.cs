namespace AdsPortal.Application.Operations.CategoryOperations.Commands.CreateCategory
{
    using System.Threading;
    using System.Threading.Tasks;
    using AdsPortal.Application.GenericHandlers.Relational.Commands;
    using AdsPortal.Application.Interfaces.Persistence.UoW;
    using AdsPortal.WebApi.Domain.Entities;
    using AutoMapper;
    using MediatR.GenericOperations.Commands;
    using MediatR.GenericOperations.Mapping;

    public class CreateCategoryCommand : ICreateCommand
    {
        public string? Name { get; init; } = string.Empty;
        public string? Description { get; init; } = string.Empty;

        void ICustomMapping.CreateMappings(Profile configuration)
        {
            configuration.CreateMap<CreateCategoryCommand, Category>();
        }

        private class Handler : CreateCommandHandler<CreateCategoryCommand, CreateCategoryValidator, Category>
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
