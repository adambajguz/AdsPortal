namespace AdsPortal.WebApi.Application.Interfaces.Persistence
{
    using System;
    using AdsPortal.WebApi.Domain.Abstractions.Base;
    using AdsPortal.WebApi.Domain.Interfaces.Repository.Generic;

    public interface IRepositoryFactory
    {
        /// <summary>
        /// Gets entity type from table name or throws exception when table not found.
        /// </summary>
        Type GetEntityTypeFromTableName(string tableName);

        /// <summary>
        /// Gets entity table name or throws exception when table not found.
        /// </summary>
        string GetTableNameFromEntityType<TEntity>()
            where TEntity : IBaseRelationalEntity;

        /// <summary>
        /// Gets entity table name or throws exception when table not found.
        /// </summary>
        string GetTableNameFromEntityType(Type entityType);

        Type GetReadOnlyRepositoryType(string tableName);
        Type GetReadOnlyRepositoryType<TEntity>() where TEntity : class, IBaseRelationalEntity;
        Type GetRepositoryType(string tableName);
        Type GetRepositoryType<TEntity>() where TEntity : class, IBaseRelationalEntity;

        IGenericRelationalReadOnlyRepository CreateReadOnlyRepository(string tableName);
        IGenericRelationalReadOnlyRepository<TEntity> CreateReadOnlyRepository<TEntity>() where TEntity : class, IBaseRelationalEntity;

        IGenericRelationalRepository CreateRepository(string tableName);
        IGenericRelationalRepository<TEntity> CreateRepository<TEntity>() where TEntity : class, IBaseRelationalEntity;

        public TSpecificRepositoryInterface CreateSpecificRepository<TSpecificRepositoryInterface>()
            where TSpecificRepositoryInterface : IGenericRelationalReadOnlyRepository;
    }
}