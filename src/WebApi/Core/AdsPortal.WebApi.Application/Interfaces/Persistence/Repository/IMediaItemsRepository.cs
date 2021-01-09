namespace AdsPortal.WebApi.Application.Interfaces.Persistence.Repository
{
    using System;
    using System.Linq.Expressions;
    using System.Threading;
    using System.Threading.Tasks;
    using AdsPortal.WebApi.Application.Interfaces.Persistence.Repository.Generic;
    using AdsPortal.WebApi.Domain.Entities;
    using AdsPortal.WebApi.Domain.Models;

    public interface IMediaItemsRepository : IGenericRelationalRepository<MediaItem>
    {
        #region ByteSize Statistics
        Task<long> GetTotalByteSizeAsync();
        Task<long> GetTotalByteSizeAsync(Expression<Func<MediaItem, bool>> filter);

        Task<double> GetAverageByteSizeAsync();
        Task<double> GetAverageByteSizeAsync(Expression<Func<MediaItem, bool>> filter);

        Task<long> GetMinByteSizeAsync();
        Task<long> GetMinByteSizeAsync(Expression<Func<MediaItem, bool>> filter);

        Task<long> GetMaxByteSizeAsync();
        Task<long> GetMaxByteSizeAsync(Expression<Func<MediaItem, bool>> filter);

        Task<StatisticsModel<long>> GetByteSizeStatisticsAsync(CancellationToken cancellationToken = default);
        Task<StatisticsModel<long>> GetByteSizeStatisticsAsync(Expression<Func<MediaItem, bool>> filter,
                                                               CancellationToken cancellationToken = default);
        #endregion
    }
}
