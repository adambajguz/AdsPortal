namespace MagicOperations.Builder
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using MagicOperations;
    using MagicOperations.Components;
    using MagicOperations.Components.Defaults;
    using MagicOperations.Components.Defaults.OperationRenderers;
    using MagicOperations.Components.Defaults.PropertyRenderers;
    using MagicOperations.Components.OperationRenderers;
    using MagicOperations.Extensions;
    using MagicOperations.Schemas;

    /// <summary>
    /// Builds an instance of <see cref="CliApplication"/>.
    /// </summary>
    public sealed class MagicOperationsBuilder
    {
        private string? _baseUri;

        private readonly Dictionary<Type, Type> _defaultOperationRenderers = new()
        {
            { typeof(CreateOperationRenderer<,>), typeof(CreateDefaultRenderer<,>) },
            { typeof(UpdateOperationRenderer<,>), typeof(UpdateDefaultRenderer<,>) },
            { typeof(DeleteOperationRenderer<,>), typeof(DeleteDefaultRenderer<,>) },
            { typeof(DetailsOperationRenderer<,>), typeof(DetailsDefaultRenderer<,>) },
            { typeof(GetAllOperationRenderer<,>), typeof(GetAllDefaultRenderer<,>) },
            { typeof(GetPagedOperationRenderer<,>), typeof(GetPagedDefaultRenderer<,>) }
        };

        private readonly Dictionary<Type, Type> _defaultPropertyRenderers = new()
        {
            { typeof(int), typeof(IntRenderer) },
            { typeof(bool), typeof(BoolRenderer) },
            { typeof(DateTime), typeof(DateTimeRenderer) },
            { typeof(string), typeof(StringRenderer) },
            { typeof(IEnumerable), typeof(CollectionRenderer) }
        };

        private readonly Dictionary<string, MagicOperationGroupConfiguration> _groupConfigurations = new();
        private readonly List<Type> _operationTypes = new List<Type>();
        private readonly List<Type> _renderableClassesTypes = new List<Type>();

        private Type? _operationListingRenderer = typeof(DefaultOperationListingRenderer);
        private Type? _errorRenderer;
        private Type? _modelRenderer;
        private Type? _anyPropertyRenderer;

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

        #region Model renderer
        public MagicOperationsBuilder UseModelRenderer(Type modelRender)
        {
            if (!modelRender.IsSubclassOf(typeof(OperationErrorRenderer)))
                throw new MagicOperationsException($"{modelRender.FullName} is not a valid operation error renderer type.");

            _modelRenderer = modelRender;

            return this;
        }

        public MagicOperationsBuilder UseModelRenderer<T>()
            where T : OperationErrorRenderer
        {
            return UseModelRenderer(typeof(T));
        }
        #endregion

        #region Default operation renderers
        public MagicOperationsBuilder UseDefaultOperationRenderer(Type baseOperation, Type renderer)
        {
            if (baseOperation == renderer)
                throw new MagicOperationsException($"Operation type cannot be equal to renderer type ({renderer.FullName}).");

            if (!baseOperation.IsSubclassOf(typeof(OperationRenderer<,>)))
                throw new MagicOperationsException($"{baseOperation.FullName} is not a valid operation renderer type.");

            if (!renderer.IsSubclassOf(baseOperation))
                throw new MagicOperationsException($"{renderer.FullName} is not a valid {baseOperation.FullName} operation renderer type.");

            _defaultOperationRenderers[baseOperation] = renderer;

            return this;
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

        #region Any property renderer
        public MagicOperationsBuilder UseAnyPropertyRenderer(Type rendererType)
        {
            if (!rendererType.IsGenericType || !rendererType.IsSubclassOf(typeof(OperationPropertyRenderer<>)))
                throw new MagicOperationsException($"{rendererType.FullName} is not a valid any operation property renderer type.");

            _anyPropertyRenderer = rendererType;

            return this;
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
        /// Adds a renderable class to the application.
        /// </summary>
        public MagicOperationsBuilder AddRenderableClass(Type type)
        {
            if (!KnownTypesHelpers.IsRenderableClassType(type))
                throw new MagicOperationsException($"{type.FullName} is not a valid renderable class type.");

            _renderableClassesTypes.Add(type);

            return this;
        }

        /// <summary>
        /// Adds a renderable class to the application.
        /// </summary>
        public MagicOperationsBuilder AddRenderableClass<T>()
            where T : class
        {
            return AddRenderableClass(typeof(T));
        }

        /// <summary>
        /// Adds multiple renderable classes to the application.
        /// </summary>
        public MagicOperationsBuilder AddRenderableClasses(IEnumerable<Type> types)
        {
            foreach (Type type in types)
                AddRenderableClass(type);

            return this;
        }

        /// <summary>
        /// Adds renderable classes from the specified assembly to the application.
        /// Only adds public valid renderable classes.
        /// Generic classes will not be added, unless fully defined in operation ResponseType or in configuration.
        /// </summary>
        public MagicOperationsBuilder AddRenderableClassesFrom(Assembly assembly)
        {
            foreach (Type type in assembly.ExportedTypes.Where(KnownTypesHelpers.IsRenderableClassType))
                AddRenderableClass(type);

            return this;
        }

        /// <summary>
        /// Adds renderable classes from the specified assemblies to the application.
        /// Only adds public valid renderable types classes.
        /// Generic classes will not be added, unless fully defined in operation ResponseType or in configuration.
        /// </summary>
        public MagicOperationsBuilder AddRenderableClassesFrom(IEnumerable<Assembly> operationAssemblies)
        {
            foreach (Assembly assembly in operationAssemblies)
                AddRenderableClassesFrom(assembly);

            return this;
        }

        /// <summary>
        /// Adds renderable classes from the calling assembly to the application.
        /// Only adds public valid renderable classes.
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
            _modelRenderer ??= typeof(DefaultModelRenderer<>);
            _anyPropertyRenderer ??= typeof(AnyRenderer<>);

            var resolvedOperations = OperationSchemaResolver.Resolve(_operationTypes, _groupConfigurations);

            IEnumerable<Type> responseTypes = resolvedOperations.OperationTypeToSchemaMap.Values.Select(x => x.ResponseType).Where(x => x is not null)!;
            IEnumerable<Type> allRenderableTypes = _renderableClassesTypes.Union(responseTypes).Distinct();

            IReadOnlyDictionary<Type, RenderableClassSchema> resolvedRenderableClasses = RenderableClassSchemaResolver.Resolve(allRenderableTypes);

            MagicOperationsConfiguration configuration = new(_baseUri,
                                                             _operationTypes,
                                                             resolvedOperations.Groups,
                                                             resolvedOperations.OperationTypeToSchemaMap,
                                                             resolvedRenderableClasses,
                                                             _operationListingRenderer,
                                                             _errorRenderer,
                                                             _modelRenderer,
                                                             _anyPropertyRenderer,
                                                             _defaultOperationRenderers,
                                                             _defaultPropertyRenderers);

            return configuration;
        }
    }
}
