namespace MagicOperations.Builder
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using MagicModels;
    using MagicModels.Builder;
    using MagicModels.Extensions;
    using MagicOperations;
    using MagicOperations.Attributes;
    using MagicOperations.Components;
    using MagicOperations.Components.Defaults;
    using MagicOperations.Components.Defaults.OperationRenderers;
    using MagicOperations.Components.OperationRenderers;
    using MagicOperations.Extensions;
    using MagicOperations.Internal.Schemas;
    using MagicOperations.Schemas;

    /// <summary>
    /// Builds an instance of <see cref="CliApplication"/>.
    /// </summary>
    public sealed class MagicOperationsBuilder
    {
        public MagicModelsBuilder ModelsBuilder { get; }

        private string? _baseUri;

        private readonly Dictionary<Type, Type> _defaultOperationRenderers = new()
        {
            { typeof(LoginOperationRenderer<,>), typeof(LoginDefaultRenderer<,>) },
            { typeof(CreateOperationRenderer<,>), typeof(CreateDefaultRenderer<,>) },
            { typeof(UpdateOperationRenderer<,>), typeof(UpdateDefaultRenderer<,>) },
            { typeof(DeleteOperationRenderer<,>), typeof(DeleteDefaultRenderer<,>) },
            { typeof(DetailsOperationRenderer<,>), typeof(DetailsDefaultRenderer<,>) },
            { typeof(GetAllOperationRenderer<,>), typeof(GetAllDefaultRenderer<,>) },
            { typeof(GetPagedOperationRenderer<,>), typeof(GetPagedDefaultRenderer<,>) }
        };

        private readonly Dictionary<string, MagicOperationGroupConfiguration> _groupConfigurations = new();
        private readonly List<Func<Type>> _operationTypes = new List<Func<Type>>();

        private Type? _operationListingRenderer = typeof(DefaultOperationListingRenderer);
        private Type? _errorRenderer;
        private string? _panelPath;

        /// <summary>
        /// Initializes an instance of <see cref="MagicOperationsBuilder"/>.
        /// </summary>
        public MagicOperationsBuilder(MagicModelsBuilder? modelsBuilder = null)
        {
            ModelsBuilder = modelsBuilder ?? new();
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
            {
                throw new MagicOperationsException($"{operationListingRenderer.FullName} is not a valid operation listing renderer type.");
            }

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
            {
                throw new MagicOperationsException($"{errorRenderer.FullName} is not a valid operation error renderer type.");
            }

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
        public MagicOperationsBuilder UseDefaultOperationRenderer(Type baseOperation, Type renderer)
        {
            if (baseOperation == renderer)
            {
                throw new MagicOperationsException($"Operation type cannot be equal to renderer type ({renderer.FullName}).");
            }

            if (!baseOperation.IsSubclassOfGeneric(typeof(OperationRenderer<,>)))
            {
                throw new MagicOperationsException($"{baseOperation.FullName} is not a valid base operation renderer type.");
            }

            if (!renderer.IsSubclassOfGeneric(baseOperation))
            {
                throw new MagicOperationsException($"{renderer.FullName} is not a valid {baseOperation.FullName} base operation renderer type.");
            }

            _defaultOperationRenderers[baseOperation] = renderer;

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
            {
                throw new MagicOperationsException($"{operationType.FullName} is not a valid operation type.");
            }

            _operationTypes.Add(() =>
            {
                ModelsBuilder.AddRenderableClass(operationType);

                OperationAttribute? operationAttr = operationType.GetCustomAttribute<OperationAttribute>(true);
                _ = operationAttr ?? throw new MagicOperationsException($"Operation {operationType.FullName} does not have {typeof(OperationAttribute).FullName} attribute.");

                if (operationAttr.ResponseType is not null)
                {
                    ModelsBuilder.AddRenderableClass(operationAttr.ResponseType);
                }

                return operationType;
            });

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
            {
                AddOperation(commandType);
            }

            return this;
        }

        /// <summary>
        /// Adds operations from the specified assembly to the application.
        /// Only adds public valid operation types.
        /// </summary>
        public MagicOperationsBuilder AddOperationsFrom(Assembly operationAssembly)
        {
            foreach (Type commandType in operationAssembly.ExportedTypes.Where(KnownTypesHelpers.IsOperationType))
            {
                AddOperation(commandType);
            }

            return this;
        }

        /// <summary>
        /// Adds operations from the specified assemblies to the application.
        /// Only adds public valid operation types.
        /// </summary>
        public MagicOperationsBuilder AddOperationsFrom(IEnumerable<Assembly> operationAssemblies)
        {
            foreach (Assembly commandAssembly in operationAssemblies)
            {
                AddOperationsFrom(commandAssembly);
            }

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

        #region Group configurations
        public MagicOperationsBuilder AddGroupConfiguration(string groupKey, Action<MagicOperationGroupConfiguration> configuration)
        {
            MagicOperationGroupConfiguration groupConfiguration = new();
            configuration?.Invoke(groupConfiguration);

            _groupConfigurations.Add(groupKey, groupConfiguration);

            return this;
        }
        #endregion

        public (MagicModelsConfiguration, MagicOperationsConfiguration) Build()
        {
            _ = _baseUri ?? throw new MagicOperationsException($"Base URI not set.");

            if (_operationTypes.Count == 0)
            {
                throw new MagicOperationsException("At least one operation must be defined in the application.");
            }

            _errorRenderer ??= typeof(DefaultErrorRenderer);
            _panelPath ??= "/panel";

            IReadOnlyList<Type> operationTypes = _operationTypes.Select(x => x()).ToList();
            MagicModelsConfiguration modelsConfiguration = ModelsBuilder.Build();

            var resolvedOperations = OperationSchemaResolver.Resolve(operationTypes, _groupConfigurations, modelsConfiguration);

            MagicOperationsConfiguration configuration = new(modelsConfiguration,
                                                             _baseUri,
                                                             _panelPath,
                                                             operationTypes,
                                                             resolvedOperations.Groups,
                                                             resolvedOperations.OperationTypeToSchemaMap,
                                                             _operationListingRenderer,
                                                             _errorRenderer,
                                                             _defaultOperationRenderers);

            return (modelsConfiguration, configuration);
        }
    }
}
