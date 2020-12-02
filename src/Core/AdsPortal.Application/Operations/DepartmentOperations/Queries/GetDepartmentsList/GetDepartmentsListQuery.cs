namespace AdsPortal.Application.Operations.DepartmentOperations.Queries.GetDepartmentsList
{
    using System.Threading;
    using System.Threading.Tasks;
    using AdsPortal.Application.GenericHandlers.Relational.Queries;
    using AdsPortal.Application.Interfaces.Persistence.UoW;
    using AdsPortal.Domain.Entities;

    public class GetDepartmentsListQuery : IGetListQuery<GetDepartmentsListResponse>
    {
        public GetDepartmentsListQuery()
        {

        }

        private class Handler : GetListQueryHandler<GetDepartmentsListQuery, Department, GetDepartmentsListResponse>
        {
            public Handler(IAppRelationalUnitOfWork uow) : base(uow)
            {

            }

            protected override Task OnInit(CancellationToken cancellationToken)
            {
                return Task.CompletedTask;
            }
        }
    }
}

