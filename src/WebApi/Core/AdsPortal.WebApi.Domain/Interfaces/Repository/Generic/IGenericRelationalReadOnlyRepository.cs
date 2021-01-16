namespace AdsPortal.WebApi.Domain.Interfaces.Repository.Generic
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading;
    using System.Threading.Tasks;
    using AdsPortal.WebApi.Domain.Abstractions.Base;

    public interface IGenericRelationalReadOnlyRepository
    {
        public Type EntityType { get; }
        public string EntityName { get; }

        Task<List<IBaseRelationalEntity>> AllAsync(bool noTracking = false,
                                                   int? skip = null,
                                                   int? take = null,
                                                   CancellationToken cancellationToken = default);

        Task<IBaseRelationalEntity> SingleByIdAsync(Guid id, bool noTracking = false, CancellationToken cancellationToken = default);
        Task<IBaseRelationalEntity?> SingleByIdOrDefaultAsync(Guid id, bool noTracking = false, CancellationToken cancellationToken = default);

        Task<int> GetCountAsync(CancellationToken cancellationToken = default);
        Task<bool> ExistsAsync(CancellationToken cancellationToken = default);
        Task ExistsElseThrowAsync(CancellationToken cancellationToken = default);
        Task<bool> ExistsByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task ExistsByIdElseThrowAsync(Guid id, CancellationToken cancellationToken = default);

        #region ProjectTo
        Task<T> ProjectedSingleAsync<T>(Expression<Func<T, bool>>? filter = null,
                                        bool noTracking = false,
                                        CancellationToken cancellationToken = default);
        Task<T?> ProjectedSingleOrDefaultAsync<T>(Expression<Func<T, bool>>? filter = null,
                                                  bool noTracking = false,
                                                  CancellationToken cancellationToken = default);

        Task<T> ProjectedFirstAsync<T>(Expression<Func<T, bool>>? filter = null,
                                       bool noTracking = false,
                                       CancellationToken cancellationToken = default);
        Task<T?> ProjectedFirstOrDefaultAsync<T>(Expression<Func<T, bool>>? filter = null,
                                                 bool noTracking = false,
                                                 CancellationToken cancellationToken = default);

        Task<T> ProjectedSingleByIdAsync<T>(Guid id, bool noTracking = false, CancellationToken cancellationToken = default);
        Task<T?> ProjectedSingleByIdOrDefaultAsync<T>(Guid id, bool noTracking = false, CancellationToken cancellationToken = default);
        #endregion
    }

    public interface IGenericRelationalReadOnlyRepository<TEntity> : IGenericRelationalReadOnlyRepository
        where TEntity : class, IBaseRelationalEntity
    {
        Task<List<TEntity>> AllAsync(Expression<Func<TEntity, bool>>? filter = null,
                                     Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
                                     bool noTracking = false,
                                     int? skip = null,
                                     int? take = null,
                                     CancellationToken cancellationToken = default);

        Task<TEntity> SingleAsync(Expression<Func<TEntity, bool>>? filter = null,
                                  bool noTracking = false,
                                  CancellationToken cancellationToken = default);
        Task<TEntity?> SingleOrDefaultAsync(Expression<Func<TEntity, bool>>? filter = null,
                                            bool noTracking = false,
                                            CancellationToken cancellationToken = default);

        Task<TEntity> FirstAsync(Expression<Func<TEntity, bool>>? filter = null,
                                 bool noTracking = false,
                                 CancellationToken cancellationToken = default);
        Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>>? filter = null,
                                           bool noTracking = false,
                                           CancellationToken cancellationToken = default);

        new Task<TEntity> SingleByIdAsync(Guid id, bool noTracking = false, CancellationToken cancellationToken = default);
        new Task<TEntity?> SingleByIdOrDefaultAsync(Guid id, bool noTracking = false, CancellationToken cancellationToken = default);

        Task<TEntity> SingleByIdWithRelatedAsync<TProperty0>(Guid id,
                                                             Expression<Func<TEntity, TProperty0>> relatedSelector0,
                                                             bool noTracking = false,
                                                             CancellationToken cancellationToken = default);
        Task<TEntity> SingleByIdWithRelatedAsync<TProperty0, TProperty1>(Guid id,
                                                                         Expression<Func<TEntity, TProperty0>> relatedSelector0,
                                                                         Expression<Func<TEntity, TProperty1>> relatedSelector1,
                                                                         bool noTracking = false,
                                                                         CancellationToken cancellationToken = default);
        Task<TEntity> SingleByIdWithRelatedAsync(Guid id,
                                                 bool noTracking = false,
                                                 CancellationToken cancellationToken = default,
                                                 params Expression<Func<TEntity, object>>[] relatedSelectors);

        Task<TEntity?> SingleByIdWithRelatedOrDefaultAsync<TProperty0>(Guid id,
                                                                       Expression<Func<TEntity, TProperty0>> relatedSelector0,
                                                                       bool noTracking = false,
                                                                       CancellationToken cancellationToken = default);
        Task<TEntity?> SingleByIdWithRelatedOrDefaultAsync<TProperty0, TProperty1>(Guid id,
                                                                                   Expression<Func<TEntity, TProperty0>> relatedSelector0,
                                                                                   Expression<Func<TEntity, TProperty1>> relatedSelector1,
                                                                                   bool noTracking = false,
                                                                                   CancellationToken cancellationToken = default);
        Task<TEntity?> SingleByIdWithRelatedOrDefaultAsync(Guid id,
                                                           bool noTracking = false,
                                                           CancellationToken cancellationToken = default,
                                                           params Expression<Func<TEntity, object>>[] relatedSelectors);

        Task<int> GetCountAsync(Expression<Func<TEntity, bool>>? filter = null, CancellationToken cancellationToken = default);
        Task<bool> ExistsAsync(Expression<Func<TEntity, bool>>? filter = null, CancellationToken cancellationToken = default);
        Task ExistsElseThrowAsync(Expression<Func<TEntity, bool>>? filter = null, CancellationToken cancellationToken = default);

        #region ProjectTo
        Task<T> ProjectedSingleAsync<T>(Expression<Func<TEntity, bool>> preFilter,
                                        Expression<Func<T, bool>>? filter = null,
                                        bool noTracking = false,
                                        CancellationToken cancellationToken = default);
        Task<T?> ProjectedSingleOrDefaultAsync<T>(Expression<Func<TEntity, bool>> preFilter,
                                                  Expression<Func<T, bool>>? filter = null,
                                                  bool noTracking = false,
                                                  CancellationToken cancellationToken = default);

        Task<T> ProjectedFirstAsync<T>(Expression<Func<TEntity, bool>> preFilter,
                                       Expression<Func<T, bool>>? filter = null,
                                       bool noTracking = false,
                                       CancellationToken cancellationToken = default);
        Task<T?> ProjectedFirstOrDefaultAsync<T>(Expression<Func<TEntity, bool>> preFilter,
                                                 Expression<Func<T, bool>>? filter = null,
                                                 bool noTracking = false,
                                                 CancellationToken cancellationToken = default);
        Task<T> ProjectedSingleByIdWithRelatedAsync<T>(Guid id,
                                                       bool noTracking = false,
                                                       CancellationToken cancellationToken = default,
                                                       params Expression<Func<TEntity, object>>[] relatedSelectors);

        Task<T?> ProjectedSingleByIdWithRelatedOrDefaultAsync<T>(Guid id,
                                                                 bool noTracking = false,
                                                                 CancellationToken cancellationToken = default,
                                                                 params Expression<Func<TEntity, object>>[] relatedSelectors);

        Task<List<T>> ProjectedAllAsync<T>(Expression<Func<TEntity, bool>>? filter = null,
                                           Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
                                           bool noTracking = false,
                                           int? skip = null,
                                           int? take = null,
                                           CancellationToken cancellationToken = default);

        Task<List<T>> ProjectedAllWithRelatedAsync<T>(Expression<Func<TEntity, bool>>? filter = null,
                                                      Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
                                                      bool noTracking = false,
                                                      int? skip = null,
                                                      int? take = null,
                                                      CancellationToken cancellationToken = default,
                                                      params Expression<Func<TEntity, object>>[] relatedSelectors);
        #endregion
    }
}
