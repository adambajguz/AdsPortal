namespace AdsPortal.WebApi.Persistence.Repository
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading;
    using System.Threading.Tasks;
    using AdsPortal.WebApi.Application.Interfaces.Identity;
    using AdsPortal.WebApi.Domain.Entities;
    using AdsPortal.WebApi.Domain.Interfaces.Repository;
    using AdsPortal.WebApi.Domain.Models;
    using AdsPortal.WebApi.Domain.Models.MediaItem;
    using AdsPortal.WebApi.Persistence.Interfaces.DbContext;
    using AdsPortal.WebApi.Persistence.Repository.Generic;
    using AutoMapper;
    using Microsoft.EntityFrameworkCore;

    public class MediaItemsRepository : GenericRelationalRepository<MediaItem>, IMediaItemsRepository
    {
        //TODO: maybe do not force user to manualy inject di services
        public MediaItemsRepository(ICurrentUserService currentUserService,
                                    IRelationalDbContext context,
                                    IMapper mapper) : base(currentUserService, context, mapper)
        {

        }

        public async Task<MediaItemAccessConstraintsModel> GetConstraintsAsync(Guid id, CancellationToken cancellationToken)
        {
            MediaItemAccessConstraintsModel constraints = await ProjectedSingleByIdAsync<MediaItemAccessConstraintsModel>(id, true, cancellationToken);

            return constraints;
        }

        #region ByteSize Statistics
        public async Task<long> GetTotalByteSizeAsync(CancellationToken cancellationToken = default)
        {
            return await DbSet.SumAsync(x => x.ByteSize, cancellationToken);
        }

        public async Task<long> GetTotalByteSizeAsync(Expression<Func<MediaItem, bool>> filter, CancellationToken cancellationToken = default)
        {
            return await DbSet.Where(filter).SumAsync(x => x.ByteSize, cancellationToken);
        }

        public async Task<double> GetAverageByteSizeAsync(CancellationToken cancellationToken = default)
        {
            return await DbSet.AverageAsync(x => x.ByteSize, cancellationToken);
        }

        public async Task<double> GetAverageByteSizeAsync(Expression<Func<MediaItem, bool>> filter, CancellationToken cancellationToken = default)
        {
            return await DbSet.Where(filter).AverageAsync(x => x.ByteSize, cancellationToken);
        }

        public async Task<long> GetMinByteSizeAsync(CancellationToken cancellationToken = default)
        {
            return await DbSet.MinAsync(x => x.ByteSize, cancellationToken);
        }

        public async Task<long> GetMinByteSizeAsync(Expression<Func<MediaItem, bool>> filter, CancellationToken cancellationToken = default)
        {
            return await DbSet.Where(filter).MinAsync(x => x.ByteSize, cancellationToken);
        }

        public async Task<long> GetMaxByteSizeAsync(CancellationToken cancellationToken = default)
        {
            return await DbSet.MaxAsync(x => x.ByteSize, cancellationToken);
        }

        public async Task<long> GetMaxByteSizeAsync(Expression<Func<MediaItem, bool>> filter, CancellationToken cancellationToken = default)
        {
            return await DbSet.Where(filter).MaxAsync(x => x.ByteSize, cancellationToken);
        }

        public async Task<StatisticsModel<long>> GetByteSizeStatisticsAsync(CancellationToken cancellationToken = default)
        {
            int count = await DbSet.CountAsync(cancellationToken);

            if (count == 0)
            {
                return new StatisticsModel<long>();
            }

            Expression<Func<MediaItem, long>> selector = x => x.ByteSize;
            long sum = await DbSet.SumAsync(selector, cancellationToken);
            double average = await DbSet.AverageAsync(selector, cancellationToken);
            long min = await DbSet.MinAsync(selector, cancellationToken);
            long max = await DbSet.MaxAsync(selector, cancellationToken);

            return new StatisticsModel<long>
            {
                Count = count,
                Sum = sum,
                Average = average,
                Min = min,
                Max = max
            };
        }

        public async Task<StatisticsModel<long>> GetByteSizeStatisticsAsync(Expression<Func<MediaItem, bool>> filter,
                                                                            CancellationToken cancellationToken = default)
        {
            int count = await DbSet.CountAsync(cancellationToken);

            if (count == 0)
            {
                return new StatisticsModel<long>();
            }

            Expression<Func<MediaItem, long>> selector = x => x.ByteSize;
            long sum = await DbSet.SumAsync(selector, cancellationToken);
            double average = await DbSet.AverageAsync(selector, cancellationToken);
            long min = await DbSet.MinAsync(selector, cancellationToken);
            long max = await DbSet.MaxAsync(selector, cancellationToken);

            return new StatisticsModel<long>
            {
                Count = count,
                Sum = sum,
                Average = average,
                Min = min,
                Max = max
            };
        }
        #endregion
    }
}
