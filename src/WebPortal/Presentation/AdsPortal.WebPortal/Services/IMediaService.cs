using System;
using System.Threading;
using System.Threading.Tasks;
using AdsPortal.WebPortal.Models.MediaItem;

namespace AdsPortal.WebPortal.Services
{
    public interface IMediaService
    {
        Task<MediaItemDetails?> GetMediaDetails(Guid id, CancellationToken cancellationToken = default);
        Task<MediaItemDetails?> GetMediaDetails(Guid? id, CancellationToken cancellationToken = default);
    }
}