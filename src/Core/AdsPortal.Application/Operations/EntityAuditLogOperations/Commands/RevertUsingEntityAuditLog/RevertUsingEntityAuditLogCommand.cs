namespace AdsPortal.Application.Operations.EntityAuditLogOperations.Commands.CreateRouteLog
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using AutoMapper;
    using AdsPortal.Application.Interfaces.Persistence.UoW;
    using AdsPortal.Application.OperationsModels.Core;
    using MediatR;

    public class RevertUsingEntityAuditLogCommand : IRequest<IdResult>
    {
        public RevertUsingEntityAuditLogRequest Data { get; }

        public RevertUsingEntityAuditLogCommand(RevertUsingEntityAuditLogRequest data)
        {
            Data = data;
        }

        private class Handler : IRequestHandler<RevertUsingEntityAuditLogCommand, IdResult>
        {
            private readonly IAppRelationalUnitOfWork _uow;
            private readonly IMapper _mapper;

            public Handler(IAppRelationalUnitOfWork uow, IMapper mapper)
            {
                _uow = uow;
                _mapper = mapper;
            }

            public async Task<IdResult> Handle(RevertUsingEntityAuditLogCommand request, CancellationToken cancellationToken)
            {
                throw new NotImplementedException();

                //RevertUsingEntityAuditLogRequest data = request.Data;

                //await new RevertUsingEntityAuditLogCommandValidator().ValidateAndThrowAsync(data, cancellationToken: cancellationToken);

                //EntityAuditLog entity = _mapper.Map<EntityAuditLog>(data);
                //_uow.EntityAuditLogs.Add(entity);

                //await _uow.SaveChangesAsync(cancellationToken);

                //return new IdResponse(entity.Id);



                //if (listHasValues)
                //{
                //    string tableName = list[0].TableName;

                //    IGenericRelationalRepository repository = _uow.GetRepositoryByName(tableName);

                //    exists = false;
                //}
            }
        }
    }
}
