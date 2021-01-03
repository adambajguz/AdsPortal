namespace MagicOperations.Builder
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using MagicOperations;
    using MagicOperations.Components;
    using MagicOperations.Components.DefaultOperationRenderers;
    using MagicOperations.Components.DefaultPropertyRenderers;
    using MagicOperations.Extensions;
    using MagicOperations.Schemas;

    /// <summary>
    /// Builds an instance of <see cref="CliApplication"/>.
    /// </summary>
    public sealed class MagicOperationsBuilder
    {
        private string? _baseUri;

        private readonly Dictionary<MagicOperationTypes, Type> _defaultOperationRenderers = new()
        {
            { MagicOperationTypes.Create, typeof(CreateDefaultRenderer<,>) },
            { MagicOperationTypes.Update, typeof(UpdateDefaultRenderer<,>) },
            { MagicOperationTypes.Delete, typeof(DeleteDefaultRenderer<,>) },
            { MagicOperationTypes.Details, typeof(DetailsDefaultRenderer<,>) },
            { MagicOperationTypes.GetAll, typeof(GetAllDefaultRenderer<,>) },
            { MagicOperationTypes.GetPaged, typeof(GetPagedDefaultRenderer<,>) }
        };

        private readonly Dictionary<Type, Type> _defaultPropertyRenderers = new()
        {
            { typeof(bool), typeof(BoolRenderer) },
            { typeof(Guid), typeof(GuidRenderer) },
            { typeof(Guid?), typeof(NullableGuidRenderer) },
            { typeof(DateTime), typeof(DateTimeRenderer) },
            { typeof(string), typeof(StringRenderer) }
        };

        private readonly Dictionary<string, MagicOperationGroupConfiguration> _groupConfigurations = new();
        private readonly List<Type> _operationTypes = new List<Type>();
        private readonly List<Type> _renderableClassesTypes = new List<Type>();

        private Type? _operationListingRenderer = typeof(DefaultOperationListingRenderer);
        private Type? _errorRenderer;

        /// <summary>
        /// Initializes an instance of <see cref="MagicOperationsBuilder"/>.
        /// </summary>
        public MagicOperationsBuilder()
        {

        }

        public MagicOperationsBuilder UseBaseUri(string baseUri)
        {
            _baseUri = baseUri;

            return this;
        }

        #region Operation listing renderer
        public MagicOperationsBuilder UseOperationListing(Type operationListingRenderer)
        {
            if (!operationListingRenderer.IsSubclassOf(typeof(OperationListingRenderer)))
                throw new MagicOperationsException($"{operationListingRenderer.FullName} is not a valid operation listing renderer type.");

            _operationListingRenderer = operationListingRenderer;

            return this;
        }

        public MagicOperationsBuilder UseOperationListing<T>()
            where T : OperationListingRenderer
        {
            return UseOperationListing(typeof(T));
        }

        public MagicOperationsBuilder DisableOperationsListing()
        {
            _operationListingRenderer = null;

            return this;
        }
        #endregion

        #region Error renderer
        public MagicOperationsBuilder UseErrorRenderer(Type errorRenderer)
        {
            if (!errorRenderer.IsSubclassOf(typeof(OperationErrorRenderer)))
                throw new MagicOperationsException($"{errorRenderer.FullName} is not a valid operation error renderer type.");

            _errorRenderer = errorRenderer;

            return this;
        }

        public MagicOperationsBuilder UseErrorRenderer<T>()
            where T : OperationErrorRenderer
        {
            return UseErrorRenderer(typeof(T));
        }
        #endregion

        #region Default operation renderers
        public MagicOperationsBuilder UseDefaultOperationRenderer(MagicOperationTypes operation, Type type)
        {
            if (!type.IsSubclassOf(typeof(OperationRenderer<,>)))
                throw new MagicOperationsException($"{type.FullName} is not a valid operation renderer type.");

            _defaultOperationRenderers[operation] = type;

            return this;
        }

        public MagicOperationsBuilder UseDefaultOperationRenderer<TRenderer>(MagicOperationTypes operation)
        {
            return UseDefaultOperationRenderer(operation, typeof(TRenderer));
        }
        #endregion

        #region Default property renderers
        public MagicOperationsBuilder UseDefaultPropertyRenderer(Type propertyType, Type rendererType)
        {
            if (!rendererType.IsSubclassOf(typeof(OperationPropertyRenderer<>).MakeGenericType(propertyType)))
                throw new MagicOperationsException($"{rendererType.FullName} is not a valid operation property renderer type.");

            _defaultPropertyRenderers[propertyType] = rendererType;

            return this;
        }

        public MagicOperationsBuilder UseDefaultPropertyRenderer<TPropety, TRenderer>()
            where TRenderer : OperationPropertyRenderer<TPropety>
        {
            return UseDefaultPropertyRenderer(typeof(TPropety), typeof(TRenderer));
        }
        #endregion

        #region Operations
        /// <summary>
        /// Adds an operation of specified type to the application.
        /// </summary>
        public MagicOperationsBuilder AddOperation(Type operationType)
        {
            if (!KnownTypesHelpers.IsOperationType(operationType))
                throw new MagicOperationsException($"{operationType.FullName} is not a valid operation type.");

            _operationTypes.Add(operationType);
            _renderableClassesTypes.Add(operationType);

            return this;
        }

        /// <summary>
        /// Adds an operation of specified type to the application.
        /// </summary>
        public MagicOperationsBuilder AddOperation<T>()
            where T : class
        {
            return AddOperation(typeof(T));
        }

        /// <summary>
        /// Adds multiple operations to the application.
        /// </summary>
        public MagicOperationsBuilder AddOperations(IEnumerable<Type> operationTypes)
        {
            foreach (Type commandType in operationTypes)
                AddOperation(commandType);

            return this;
        }

        /// <summary>
        /// Adds operations from the specified assembly to the application.
        /// Only adds public valid operation types.
        /// </summary>
        public MagicOperationsBuilder AddOperationsFrom(Assembly operationAssembly)
        {
            foreach (Type commandType in operationAssembly.ExportedTypes.Where(KnownTypesHelpers.IsOperationType))
                AddOperation(commandType);

            return this;
        }

        /// <summary>
        /// Adds operations from the specified assemblies to the application.
        /// Only adds public valid operation types.
        /// </summary>
        public MagicOperationsBuilder AddOperationsFrom(IEnumerable<Assembly> operationAssemblies)
        {
            foreach (Assembly commandAssembly in operationAssemblies)
                AddOperationsFrom(commandAssembly);

            return this;
        }

        /// <summary>
        /// Adds operations from the calling assembly to the application.
        /// Only adds public valid operation types.
        /// </summary>
        public MagicOperationsBuilder AddOperationsFromThisAssembly()
        {
            return AddOperationsFrom(Assembly.GetCallingAssembly());
        }
        #endregion

        #region Renderable class
        /// <summary>
        /// Adds an operation of specified type to the application.
        /// </summary>
        public MagicOperationsBuilder AddRenderableClass(Type type)
        {
            if (!KnownTypesHelpers.IsRenderableClassType(type))
                throw new MagicOperationsException($"{type.FullName} is not a valid renderable class type.");

            _renderableClassesTypes.Add(type);

            return this;
        }

        /// <summary>
        /// Adds an operation of specified type to the application.
        /// </summary>
        public MagicOperationsBuilder AddRenderableClass<T>()
            where T : class
        {
            return AddRenderableClass(typeof(T));
        }

        /// <summary>
        /// Adds multiple operations to the application.
        /// </summary>
        public MagicOperationsBuilder AddRenderableClasses(IEnumerable<Type> operationTypes)
        {
            foreach (Type commandType in operationTypes)
                AddRenderableClass(commandType);

            return this;
        }

        /// <summary>
        /// Adds operations from the specified assembly to the application.
        /// Only adds public valid operation types.
        /// </summary>
        public MagicOperationsBuilder AddRenderableClassesFrom(Assembly operationAssembly)
        {
            foreach (Type commandType in operationAssembly.ExportedTypes.Where(KnownTypesHelpers.IsOperationType))
                AddRenderableClass(commandType);

            return this;
        }

        /// <summary>
        /// Adds operations from the specified assemblies to the application.
        /// Only adds public valid operation types.
        /// </summary>
        public MagicOperationsBuilder AddRenderableClassesFrom(IEnumerable<Assembly> operationAssemblies)
        {
            foreach (Assembly commandAssembly in operationAssemblies)
                AddRenderableClassesFrom(commandAssembly);

            return this;
        }

        /// <summary>
        /// Adds operations from the calling assembly to the application.
        /// Only adds public valid operation types.
        /// </summary>
        public MagicOperationsBuilder AddRenderableClassesFromThisAssembly()
        {
            return AddRenderableClassesFrom(Assembly.GetCallingAssembly());
        }
        #endregion

        #region Group configurations
        public MagicOperationsBuilder AddGroupConfiguration(string groupKey, Action<MagicOperationGroupConfiguration> configuration)
        {
            MagicOperationGroupConfiguration groupConfiguration = new();
            configuration?.Invoke(groupConfiguration);

            _groupConfigurations.Add(groupKey, groupConfiguration);

            return this;
        }
        #endregion

        /// <summary>
        /// Creates an instance of <see cref="CliApplication"/> using configured parameters.
        /// Default values are used in place of parameters that were not specified.
        /// A scope is defined as a lifetime of a command execution pipeline that includes directives handling.
        /// </summary>
        public MagicOperationsConfiguration Build()
        {
            _ = _baseUri ?? throw new MagicOperationsException($"Base URI not set.");

            if (_operationTypes.Count == 0)
                throw new MagicOperationsException("At least one operation must be defined in the application.");

            _errorRenderer ??= typeof(DefaultErrorRenderer);

            var resolvedOperations = OperationSchemaResolver.Resolve(_operationTypes, _groupConfigurations);
            IReadOnlyDictionary<Type, RenderableClassSchema> resolvedRenderableClasses = RenderableClassSchemaResolver.Resolve(_renderableClassesTypes.Distinct());

            MagicOperationsConfiguration configuration = new(_baseUri,
                                                             _operationTypes,
                                                             resolvedOperations.Groups,
                                                             resolvedOperations.OperationTypeToSchemaMap,
                                                             resolvedRenderableClasses,
                                                             _operationListingRenderer,
                                                             _errorRenderer,
                                                             _defaultOperationRenderers,
                                                             _defaultPropertyRenderers);

            return configuration;
        }
    }
}
