namespace AdsPortal.Application.Operations.CategoryOperations.Commands.UpdateCategory
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using AdsPortal.Application.GenericHandlers.Relational.Commands;
    using AdsPortal.Application.Interfaces.Persistence.UoW;
    using AdsPortal.Domain.Entities;
    using AdsPortal.Domain.Mapping;
    using AutoMapper;

    public class UpdateCategoryCommand : IUpdateCommand
    {
        public Guid Id { get; init; }

        public string? Name { get; init; } = string.Empty;
        public string? Description { get; init; } = string.Empty;

        void ICustomMapping.CreateMappings(Profile configuration)
        {
            configuration.CreateMap<UpdateCategoryCommand, Category>();
        }

        private class Handler : UpdateCommandHandler<UpdateCategoryCommand, UpdateCategoryValidator, Category>
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
