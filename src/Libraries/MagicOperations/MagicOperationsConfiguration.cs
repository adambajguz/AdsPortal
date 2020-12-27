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
        public IReadOnlyList<OperationSchema> OperationSchemas { get; }

        public IReadOnlyDictionary<MagicOperationTypes, Type> DefaultOperationRenderers { get; }
        public IReadOnlyDictionary<Type, Type> DefaultOperationPropertyRenderers { get; }

        public IReadOnlyDictionary<Type, OperationSchema> ModelToSchemaMappings { get; }

        public MagicOperationsConfiguration(string baseApiPath,
                                            IReadOnlyList<Type> operationTypes,
                                            IReadOnlyDictionary<string, OperationGroupSchema> operationGroups,
                                            IReadOnlyList<OperationSchema> operationSchemas,
                                            IReadOnlyDictionary<Type, OperationSchema> modelToSchemaMappings,
                                            IReadOnlyDictionary<MagicOperationTypes, Type> defaultOperationRenderers,
                                            IReadOnlyDictionary<Type, Type> defaultOperationPropertyRenderers)
        {
            BaseApiPath = baseApiPath;
            OperationTypes = operationTypes;

            OperationGroups = operationGroups;
            OperationSchemas = operationSchemas;
            ModelToSchemaMappings = modelToSchemaMappings;

            DefaultOperationRenderers = defaultOperationRenderers;
            DefaultOperationPropertyRenderers = defaultOperationPropertyRenderers;
        }
    }
}