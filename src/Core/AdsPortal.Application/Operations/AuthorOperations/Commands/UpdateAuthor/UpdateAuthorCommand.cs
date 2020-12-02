namespace AdsPortal.Application.Operations.AuthorOperations.Commands.UpdateAuthor
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using AutoMapper;
    using AdsPortal.Application.GenericHandlers.Relational.Commands;
    using AdsPortal.Application.Interfaces.Persistence.UoW;
    using AdsPortal.Domain.Entities;
    using AdsPortal.Domain.Mapping;

    public class UpdateAuthorCommand : IUpdateCommand
    {
        public Guid Id { get; set; }

        public string? Name { get; set; } = string.Empty;
        public string? Surname { get; set; } = string.Empty;
        public string? ORCID { get; set; } = string.Empty;

        void ICustomMapping.CreateMappings(Profile configuration)
        {
            configuration.CreateMap<UpdateAuthorCommand, Author>();
        }

        private class Handler : UpdateCommandHandler<UpdateAuthorCommand, UpdateAuthorValidator, Author>
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
