namespace AdsPortal.WebApi.Application.Interfaces.Persistence.Repository
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using AdsPortal.WebApi.Application.Interfaces.Persistence.Repository.Generic;
    using AdsPortal.WebApi.Domain.Entities;

    public interface ICategoriesRepository : IGenericRelationalRepository<Category>
    {
        Task<List<Category>> AllAsync_WithRelated(bool noTracking = false,
                                                  int? skip = null,
                                                  int? take = null,
                                                  CancellationToken cancellationToken = default);
    }
}
