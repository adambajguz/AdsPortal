namespace AdsPortal.Application.Operations.DegreeOperations.Queries.GetDegreeDetails
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using AutoMapper;
    using AdsPortal.Application.GenericHandlers.Relational.Queries;
    using AdsPortal.Application.Interfaces.Persistence.UoW;
    using AdsPortal.Domain.Entities;

    public class GetDegreeDetailsQuery : IGetDetailsByIdQuery<GetDegreeDetailsResponse>
    {
        public Guid Id { get; }

        public GetDegreeDetailsQuery(Guid id)
        {
            Id = id;
        }

        private class Handler : GetDetailsByIdQueryHandler<GetDegreeDetailsQuery, Degree, GetDegreeDetailsResponse>
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
