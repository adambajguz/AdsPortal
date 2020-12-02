﻿namespace AdsPortal.Application.Operations.DegreeOperations.Commands.UpdateDegree
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using AutoMapper;
    using AdsPortal.Application.GenericHandlers.Relational.Commands;
    using AdsPortal.Application.Interfaces.Persistence.UoW;
    using AdsPortal.Domain.Entities;
    using AdsPortal.Domain.Mapping;

    public class UpdateDegreeCommand : IUpdateCommand
    {
        public Guid Id { get; set; }

        public string? Name { get; set; } = string.Empty;

        void ICustomMapping.CreateMappings(Profile configuration)
        {
            configuration.CreateMap<UpdateDegreeCommand, Degree>();
        }

        private class Handler : UpdateCommandHandler<UpdateDegreeCommand, UpdateDegreeValidator, Degree>
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
