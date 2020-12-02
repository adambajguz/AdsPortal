namespace AdsPortal.Application.Operations.DepartmentOperations.Queries.GetPagedDepartmentsList
{
    using System.Threading;
    using System.Threading.Tasks;
    using AdsPortal.Application.GenericHandlers.Relational.Queries;
    using AdsPortal.Application.Interfaces.Persistence.UoW;
    using AdsPortal.Application.Operations.DepartmentOperations.Queries.GetDepartmentsList;
    using AdsPortal.Domain.Entities;

    public class GetPagedDepartmentsListQuery : IGetPagedListQuery<GetDepartmentsListResponse>
    {
        public int Page { get; set; }
        public int EntiresPerPage { get; set; }

        private class Handler : GetPagedListQueryHandler<GetPagedDepartmentsListQuery, Department, GetDepartmentsListResponse>
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
