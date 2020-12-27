namespace MagicOperations.Configurations
{
    using System;
    using System.Collections.Generic;
    using MagicOperations.Schemas;

    public class MagicOperationsConfiguration
    {
        public string BaseApiPath { get; }

        public IReadOnlyList<Type> OperationTypes { get; }
        public IReadOnlyDictionary<string, OperationGroupSchema> OperationGroups { get; }
        public IReadOnlyList<OperationSchema> OperationSchemas { get; }

        public MagicOperationsConfiguration(string baseApiPath,
                                            IReadOnlyList<Type> operationTypes,
                                            IReadOnlyDictionary<string, OperationGroupSchema> operationGroups,
                                            IReadOnlyList<OperationSchema> operationSchemas)
        {
            BaseApiPath = baseApiPath;
            OperationTypes = operationTypes;
            OperationGroups = operationGroups;
            OperationSchemas = operationSchemas;
        }
    }
}