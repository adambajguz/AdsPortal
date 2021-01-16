namespace AdsPortal.WebApi.Domain.Interfaces.Repository
{
    using System;
    using System.Linq.Expressions;
    using System.Threading;
    using System.Threading.Tasks;
    using AdsPortal.WebApi.Domain.Entities;
    using AdsPortal.WebApi.Domain.Interfaces.Repository.Generic;
    using AdsPortal.WebApi.Domain.Models;
    using AdsPortal.WebApi.Domain.Models.MediaItem;

    public interface IMediaItemsRepository : IGenericRelationalRepository<MediaItem>
    {
        Task<MediaItemAccessConstraintsModel> GetConstraintsAsync(Guid id, CancellationToken cancellationToken);
        Task<byte[]?> GetFileData(Guid id, CancellationToken cancellationToken);

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
