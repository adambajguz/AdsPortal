namespace MagicCRUD.Builder
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using AdsPortal.Domain.Abstractions.Base;
    using MagicCRUD.Configurations;

    public sealed class MagicCRUDBuilder : IBuilder<MagicCRUDConfiguration>
    {
        private readonly Dictionary<Type, Func<OperationGroupConfiguration>> _operationGroupBuilders = new();

        private string? _baseApiPath;

        internal MagicCRUDBuilder()
        {

        }

        public MagicCRUDBuilder UseBaseApiPath(string? path)
        {
            _baseApiPath = path;

            return this;
        }

        public MagicCRUDBuilder AddOperationsGroup<TEntity>(Action<OperationGroupBuilder<TEntity>> operationBuilder)
            where TEntity : IBaseIdentifiableEntity
        {
            OperationGroupBuilder<TEntity> operationGroupBuilder = new OperationGroupBuilder<TEntity>();

            _operationGroupBuilders.Add(typeof(TEntity), () =>
            {
                operationBuilder.Invoke(operationGroupBuilder);
                return operationGroupBuilder.Build();
            });

            return this;
        }

        public MagicCRUDConfiguration Build()
        {
            _baseApiPath = _baseApiPath is null ? string.Empty : _baseApiPath.Trim('/') + '/';

            return new MagicCRUDConfiguration
            {
                BaseApiPath = _baseApiPath,
                OperationGroups = _operationGroupBuilders.ToDictionary(x => x.Key, x => x.Value.Invoke())
            };
        }
    }
}
