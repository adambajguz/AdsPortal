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
        Task<long> GetTotalByteSizeAsync(CancellationToken cancellationToken = default);
        Task<long> GetTotalByteSizeAsync(Expression<Func<MediaItem, bool>> filter, CancellationToken cancellationToken = default);

        Task<double> GetAverageByteSizeAsync(CancellationToken cancellationToken = default);
        Task<double> GetAverageByteSizeAsync(Expression<Func<MediaItem, bool>> filter, CancellationToken cancellationToken = default);

        Task<long> GetMinByteSizeAsync(CancellationToken cancellationToken = default);
        Task<long> GetMinByteSizeAsync(Expression<Func<MediaItem, bool>> filter, CancellationToken cancellationToken = default);

        Task<long> GetMaxByteSizeAsync(CancellationToken cancellationToken = default);
        Task<long> GetMaxByteSizeAsync(Expression<Func<MediaItem, bool>> filter, CancellationToken cancellationToken = default);

        Task<StatisticsModel<long>> GetByteSizeStatisticsAsync(CancellationToken cancellationToken = default);
        Task<StatisticsModel<long>> GetByteSizeStatisticsAsync(Expression<Func<MediaItem, bool>> filter, CancellationToken cancellationToken = default);
        #endregion
    }
}
