namespace MagicOperations
{
    using System;
    using System.Collections.Generic;
    using MagicOperations.Schemas;

    public sealed class MagicOperationsConfiguration
    {
        public string BaseApiPath { get; }

        public IReadOnlyList<Type> OperationTypes { get; }
        public IReadOnlyDictionary<string, OperationGroupSchema> OperationGroups { get; }
        public IEnumerable<OperationSchema> OperationSchemas => OperationTypeToSchemaMap.Values;
        public IReadOnlyDictionary<Type, OperationSchema> OperationTypeToSchemaMap { get; }

        public IReadOnlyDictionary<Type, RenderableClassSchema> RenderabeTypeToSchemaMap { get; }

        public Type? OperationListingRenderer { get; }
        public Type ErrorRenderer { get; }
        public Type DefaultModelRenderer { get; }
        public IReadOnlyDictionary<MagicOperationTypes, Type> DefaultOperationRenderers { get; }
        public IReadOnlyDictionary<Type, Type> DefaultPropertyRenderers { get; }

        public MagicOperationsConfiguration(string baseApiPath,
                                            IReadOnlyList<Type> operationTypes,
                                            IReadOnlyDictionary<string, OperationGroupSchema> operationGroups,
                                            IReadOnlyDictionary<Type, OperationSchema> operationTypeToSchemaMap,

                                            IReadOnlyDictionary<Type, RenderableClassSchema> renderabeTypeToSchemaMap,

                                            Type? operationListingRenderer,
                                            Type errorRenderer,
                                            Type defaultModelRenderer,

                                            IReadOnlyDictionary<MagicOperationTypes, Type> defaultOperationRenderers,
                                            IReadOnlyDictionary<Type, Type> defaultPropertyRenderers)
        {
            BaseApiPath = baseApiPath;

            OperationTypes = operationTypes;
            OperationGroups = operationGroups;
            OperationTypeToSchemaMap = operationTypeToSchemaMap;

            RenderabeTypeToSchemaMap = renderabeTypeToSchemaMap;

            OperationListingRenderer = operationListingRenderer;
            ErrorRenderer = errorRenderer;
            DefaultModelRenderer = defaultModelRenderer;

            DefaultOperationRenderers = defaultOperationRenderers;
            DefaultPropertyRenderers = defaultPropertyRenderers;
        }
    }
}
