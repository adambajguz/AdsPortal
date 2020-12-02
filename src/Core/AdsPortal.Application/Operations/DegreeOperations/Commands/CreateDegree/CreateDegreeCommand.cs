﻿namespace AdsPortal.Application.Operations.DegreeOperations.Commands.CreateDegree
{
    using System.Threading;
    using System.Threading.Tasks;
    using AdsPortal.Application.GenericHandlers.Relational.Commands;
    using AdsPortal.Application.Interfaces.Persistence.UoW;
    using AdsPortal.Domain.Entities;
    using AdsPortal.Domain.Mapping;
    using AutoMapper;

    public class CreateDegreeCommand : ICreateCommand
    {
        public string? Name { get; set; } = string.Empty;

        void ICustomMapping.CreateMappings(Profile configuration)
        {
            configuration.CreateMap<CreateDegreeCommand, Degree>();
        }

        private class Handler : CreateCommandHandler<CreateDegreeCommand, CreateDegreeValidator, Degree>
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
