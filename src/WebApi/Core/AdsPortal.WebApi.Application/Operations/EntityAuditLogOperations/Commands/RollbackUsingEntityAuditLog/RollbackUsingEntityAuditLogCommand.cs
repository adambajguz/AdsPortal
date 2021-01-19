namespace AdsPortal.WebApi.Application.Operations.EntityAuditLogOperations.Commands.RevertUsingEntityAuditLog
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using AdsPortal.WebApi.Application.Exceptions;
    using AdsPortal.WebApi.Domain.Abstractions.Base;
    using AdsPortal.WebApi.Domain.Abstractions.Enums;
    using AdsPortal.WebApi.Domain.Entities;
    using AdsPortal.WebApi.Domain.Interfaces.Repository.Generic;
    using AdsPortal.WebApi.Domain.Interfaces.UoW;
    using AutoMapper;
    using FluentValidation;
    using MediatR;
    using Microsoft.Extensions.Logging;
    using Newtonsoft.Json;

    public sealed record RollbackUsingEntityAuditLogCommand : IRequest
    {
        public Guid EntityKey { get; init; }
        public Guid ToId { get; init; }

        //{
        //  "entityKey": "EA9EC945-BBFE-4A32-C7C9-08D8B8A7825D",
        //  "toId": "3D58B673-3F70-4C16-1198-08D8B8A7826A"
        //}

        private sealed class Handler : IRequestHandler<RollbackUsingEntityAuditLogCommand>
        {
            private readonly IAppRelationalUnitOfWork _uow;
            private readonly IMapper _mapper;
            private readonly ILogger _logger;

            public Handler(IAppRelationalUnitOfWork uow, IMapper mapper, ILogger<Handler> logger)
            {
                _uow = uow;
                _mapper = mapper;
                _logger = logger;
            }

            public async Task<Unit> Handle(RollbackUsingEntityAuditLogCommand request, CancellationToken cancellationToken)
            {
                await new RollbackUsingEntityAuditLogValidator().ValidateAndThrowAsync(request, cancellationToken: cancellationToken);

                List<EntityAuditLog> logs = await _uow.EntityAuditLogs.AllAsync(x => x.Key == request.EntityKey, orderBy: (o) => o.OrderByDescending(x => x.CreatedOn), cancellationToken: cancellationToken);

                if (logs.Count == 0)
                {
                    throw new NotFoundException(nameof(EntityAuditLog));
                }

                List<EntityAuditLog> rollbackTrace = logs.TakeWhile(x => x.Id != request.ToId).ToList();

                if (rollbackTrace.Count == 0)
                {
                    throw new NotFoundException(nameof(EntityAuditLog));
                }

                _logger.LogInformation("Rollback of entity with Id {EntityId} to time point {ToId} started.", request.EntityKey, request.ToId);

                if (rollbackTrace.Count > 0)
                {
                    string tableName = logs[0].TableName;

                    IGenericRelationalRepository repository = _uow.GetRepositoryByName(tableName);
                    IBaseRelationalEntity? currentEntity = await repository.SingleByIdOrDefaultAsync(request.EntityKey, cancellationToken: cancellationToken);

                    foreach (EntityAuditLog log in rollbackTrace)
                    {
                        AuditActions action = log.Action;
                        string? values = log.Values;

                        object? entity = values is null ? null : JsonConvert.DeserializeObject(values, repository.EntityType);

                        switch (action)
                        {
                            case AuditActions.Added:
                                break;

                            case AuditActions.Modified:
                                break;

                            case AuditActions.Deleted:
                                break;

                            case AuditActions.Rollback:
                                break;

                            default:
                                throw new NotImplementedException($"Audit action '{action}' not implemented.");
                        }
                    }

                    await _uow.SaveChangesWithoutAuditAsync(cancellationToken);
                }

                _logger.LogInformation("Rollback of entity with Id {EntityId} to time point {ToId} finished successfully.", request.EntityKey, request.ToId);

                return await Unit.Task;
            }
        }
    }
}
