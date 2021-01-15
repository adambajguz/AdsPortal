namespace AdsPortal.WebApi.Application.Operations.CategoryOperations.Commands.CreateCategory
{
    using AdsPortal.WebApi.Application.GenericHandlers.Relational.Commands;
    using AdsPortal.WebApi.Application.Interfaces.Persistence.UoW;
    using AdsPortal.WebApi.Domain.Entities;
    using AutoMapper;
    using AutoMapper.Extensions;
    using MediatR.GenericOperations.Commands;

    public sealed record CreateCategoryCommand : ICreateCommand
    {
        public string? Name { get; init; } = string.Empty;
        public string? Description { get; init; } = string.Empty;

        void ICustomMapping.CreateMappings(Profile configuration)
        {
            configuration.CreateMap<CreateCategoryCommand, Category>();
        }

        private sealed class Handler : CreateCommandHandler<CreateCategoryCommand, CreateCategoryValidator, Category>
        {
            public Handler(IAppRelationalUnitOfWork uow, IMapper mapper) : base(uow, mapper)
            {

            }
        }
    }
}
