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

        public Type? OperationListingRenderer { get; }
        public Type ErrorRenderer { get; }
        public IReadOnlyDictionary<Type, Type> DefaultOperationRenderers { get; }

        public MagicOperationsConfiguration(string baseApiPath,
                                            IReadOnlyList<Type> operationTypes,
                                            IReadOnlyDictionary<string, OperationGroupSchema> operationGroups,
                                            IReadOnlyDictionary<Type, OperationSchema> operationTypeToSchemaMap,

                                            Type? operationListingRenderer,
                                            Type errorRenderer,

                                            IReadOnlyDictionary<Type, Type> defaultOperationRenderers)
        {
            BaseApiPath = baseApiPath;

            OperationTypes = operationTypes;
            OperationGroups = operationGroups;
            OperationTypeToSchemaMap = operationTypeToSchemaMap;

            OperationListingRenderer = operationListingRenderer;
            ErrorRenderer = errorRenderer;

            DefaultOperationRenderers = defaultOperationRenderers;
        }
    }
}
