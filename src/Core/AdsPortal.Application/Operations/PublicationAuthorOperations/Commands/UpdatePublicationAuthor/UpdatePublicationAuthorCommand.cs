namespace AdsPortal.Application.Operations.PublicationAuthorOperations.Commands.UpdatePublicationAuthor
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using AdsPortal.Application.GenericHandlers.Relational.Commands;
    using AdsPortal.Application.Interfaces.Persistence.UoW;
    using AdsPortal.Domain.Entities;
    using AdsPortal.Domain.Mapping;
    using AutoMapper;

    public class UpdatePublicationAuthorCommand : IUpdateCommand
    {
        public Guid Id { get; set; }

        public string? Name { get; set; } = string.Empty;
        public string? Surname { get; set; } = string.Empty;
        public string? ORCID { get; set; } = string.Empty;

        void ICustomMapping.CreateMappings(Profile configuration)
        {
            configuration.CreateMap<UpdatePublicationAuthorCommand, PublicationAuthor>();
        }

        private class Handler : UpdateCommandHandler<UpdatePublicationAuthorCommand, UpdatePublicationAuthorValidator, PublicationAuthor>
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
