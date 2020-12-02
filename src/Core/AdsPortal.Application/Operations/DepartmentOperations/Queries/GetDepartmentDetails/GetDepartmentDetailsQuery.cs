namespace AdsPortal.Application.Operations.DepartmentOperations.Queries.GetDepartmentDetails
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using AdsPortal.Application.GenericHandlers.Relational.Queries;
    using AdsPortal.Application.Interfaces.Persistence.UoW;
    using AdsPortal.Domain.Entities;
    using AutoMapper;

    public class GetDepartmentDetailsQuery : IGetDetailsByIdQuery<GetDepartmentDetailsResponse>
    {
        public Guid Id { get; }

        public GetDepartmentDetailsQuery(Guid id)
        {
            Id = id;
        }

        private class Handler : GetDetailsByIdQueryHandler<GetDepartmentDetailsQuery, Department, GetDepartmentDetailsResponse>
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
