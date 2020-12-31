namespace AdsPortal.Application.Interfaces.Persistence.Repository
{
    using AdsPortal.Application.Interfaces.Persistence.Repository.Generic;
    using AdsPortal.WebApi.Domain.Entities;

    public interface IJobsRepository : IGenericRelationalRepository<Job>
    {

    }
}
