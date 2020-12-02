namespace AdsPortal.Application.Interfaces.Persistence.Repository
{
    using System;
    using System.Linq.Expressions;
    using System.Threading;
    using System.Threading.Tasks;
    using AdsPortal.Application.Interfaces.Persistence.Repository.Generic;
    using AdsPortal.Domain.Models;
    using Domain.Entities;

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
