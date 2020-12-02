namespace AdsPortal.Application.Operations.JournalOperations.Commands.CreateJournal
{
    using System.Threading;
    using System.Threading.Tasks;
    using AutoMapper;
    using AdsPortal.Application.GenericHandlers.Relational.Commands;
    using AdsPortal.Application.Interfaces.Persistence.UoW;
    using AdsPortal.Domain.Entities;
    using AdsPortal.Domain.Mapping;

    public class CreateJournalCommand : ICreateCommand
    {
        public string? Name { get; set; } = string.Empty;
        public string? ISSN { get; set; } = string.Empty;
        public string? EISSN { get; set; } = string.Empty;

        public double Points { get; set; }
        public string? Disciplines { get; set; } = string.Empty;

        void ICustomMapping.CreateMappings(Profile configuration)
        {
            configuration.CreateMap<CreateJournalCommand, Journal>();
        }

        private class Handler : CreateCommandHandler<CreateJournalCommand, CreateJournalValidator, Journal>
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
