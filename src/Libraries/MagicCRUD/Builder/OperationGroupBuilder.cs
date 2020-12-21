namespace MagicCRUD.Builder
{
    using System.Net.Http;
    using MagicCRUD.Configurations;
    using MediatR.GenericOperations.Abstractions;
    using MediatR.GenericOperations.Commands;
    using MediatR.GenericOperations.Mapping;
    using MediatR.GenericOperations.Queries;

    public sealed class OperationGroupBuilder<TEntity> : IBuilder<OperationGroupConfiguration>
    {
        private string? _operationPath;

        private OperationConfiguration? _createOperation;
        private OperationConfiguration? _updateOperation;
        private OperationConfiguration? _patchOperation;
        private OperationConfiguration? _deleteOperation;
        private OperationConfiguration? _getDetailsByIdOperation;
        private OperationConfiguration? _getListOperation;
        private OperationConfiguration? _getPagedListOperation;

        internal OperationGroupBuilder()
        {

        }

        public OperationGroupBuilder<TEntity> UseCustomOperationPath(string? path)
        {
            _operationPath = path;

            return this;
        }

        public OperationGroupBuilder<TEntity> AddCreateOperation<T>(string template = "create", HttpMethod? method = null, DataEmbeddings embedding = DataEmbeddings.Body)
            where T : ICreateCommand
        {
            method ??= HttpMethod.Post;
            _createOperation = new OperationConfiguration(template, method, typeof(TEntity), typeof(T), embedding);

            return this;
        }

        public OperationGroupBuilder<TEntity> AddUpdateOperation<T>(string template = "update", HttpMethod? method = null, DataEmbeddings embedding = DataEmbeddings.Body)
            where T : IUpdateCommand
        {
            method ??= HttpMethod.Put;
            _updateOperation = new OperationConfiguration(template, method, typeof(TEntity), typeof(T), embedding);

            return this;
        }

        public OperationGroupBuilder<TEntity> AddPatchOperation<T>(string template = "patch", HttpMethod? method = null, DataEmbeddings embedding = DataEmbeddings.Body)
            where T : IUpdateCommand
        {
            method ??= HttpMethod.Patch;
            _patchOperation = new OperationConfiguration(template, method, typeof(TEntity), typeof(T), embedding);

            return this;
        }

        public OperationGroupBuilder<TEntity> AddDeleteOperation<T>(string template = "delete/{id}", HttpMethod? method = null, DataEmbeddings embedding = DataEmbeddings.Path)
            where T : IDeleteByIdCommand
        {
            method ??= HttpMethod.Delete;
            _deleteOperation = new OperationConfiguration(template, method, typeof(TEntity), typeof(T), embedding);

            return this;
        }

        public OperationGroupBuilder<TEntity> AddGetDetailsByIdOperation<T, TResult>(string template = "get/{id}", HttpMethod? method = null, DataEmbeddings embedding = DataEmbeddings.Path)
            where T : IGetDetailsByIdQuery<TResult>
            where TResult : class, IIdentifiableOperationResult, ICustomMapping
        {
            method ??= HttpMethod.Get;
            _getDetailsByIdOperation = new OperationConfiguration(template, method, typeof(TEntity), typeof(T), embedding);

            return this;
        }

        public OperationGroupBuilder<TEntity> AddGetListOperation<T, TResultEntry>(string template = "get-all", HttpMethod? method = null)
            where T : IGetListQuery<TResultEntry>
            where TResultEntry : class, IIdentifiableOperationResult, ICustomMapping
        {
            method ??= HttpMethod.Get;
            _getListOperation = new OperationConfiguration(template, method, typeof(TEntity), typeof(T), DataEmbeddings.None);

            return this;
        }

        public OperationGroupBuilder<TEntity> AddGetPagedListOperation<T, TResult>(string template = "get-paged", HttpMethod? method = null, DataEmbeddings embedding = DataEmbeddings.Query)
            where T : IGetPagedListQuery<TResult>
            where TResult : class, IIdentifiableOperationResult, ICustomMapping
        {
            method ??= HttpMethod.Get;
            _getPagedListOperation = new OperationConfiguration(template, method, typeof(TEntity), typeof(T), embedding);

            return this;
        }

        public OperationGroupConfiguration Build()
        {
            _operationPath ??= typeof(TEntity).Name.ToLower();
            _operationPath = _operationPath.Trim('/') + '/';

            OperationGroupConfiguration cfg = new ()
            {
                EntityType = typeof(TEntity),
                OperationPath = _operationPath ?? string.Empty,
                CreateOperation = _createOperation,
                UpdateOperation = _updateOperation,
                PatchOperation = _patchOperation,
                DeleteOperation = _deleteOperation,
                GetDetailsByIdOperation = _getDetailsByIdOperation,
                GetListOperation = _getListOperation,
                GetPagedListOperation = _getPagedListOperation
            };

            foreach(OperationConfiguration x in cfg.GetOperations())
            {
                x.Group = cfg;
            }

            return cfg;
        }
    }
}
