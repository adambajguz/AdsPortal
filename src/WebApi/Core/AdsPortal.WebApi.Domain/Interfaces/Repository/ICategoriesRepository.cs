namespace AdsPortal.WebApi.Domain.Interfaces.Repository
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using AdsPortal.WebApi.Domain.Entities;
    using AdsPortal.WebApi.Domain.Interfaces.Repository.Generic;

    public interface ICategoriesRepository : IGenericRelationalRepository<Category>
    {
        Task<List<Category>> AllAsync_WithRelated(bool noTracking = false,
                                                  int? skip = null,
                                                  int? take = null,
                                                  CancellationToken cancellationToken = default);
    }
}
