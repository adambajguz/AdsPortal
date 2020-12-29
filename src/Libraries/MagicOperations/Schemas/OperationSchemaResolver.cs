namespace MagicOperations.Schemas
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using MagicOperations.Attributes;
    using MagicOperations.Extensions;
    using Microsoft.AspNetCore.Http;

    public static class OperationSchemaResolver
    {
        public static (IReadOnlyDictionary<string, OperationGroupSchema> Groups, IReadOnlyList<OperationSchema> Schemas, IReadOnlyDictionary<Type, OperationSchema> ModelToSchemaMappings) Resolve(IReadOnlyList<Type> operationTypes, Dictionary<string, MagicOperationGroupConfiguration> groupConfigurations)
        {
            Dictionary<string, OperationGroupSchema> groups = new();
            List<OperationSchema> schemas = new();
            Dictionary<Type, OperationSchema> modelToSchemaMappings = new();

            foreach (Type operationModelType in operationTypes)
            {
                OperationGroupAttribute? group = operationModelType.GetCustomAttribute<OperationGroupAttribute>(true);

                OperationGroupSchema groupSchema = ResolveOperationGroup(groupConfigurations, groups, group);

                OperationSchema operationSchema = ResolveOperation(operationModelType, groupSchema);
                schemas.Add(operationSchema);
                groupSchema.AddOperation(operationSchema);
                modelToSchemaMappings.Add(operationModelType, operationSchema);
            }

            return (groups, schemas, modelToSchemaMappings);
        }

        private static OperationGroupSchema ResolveOperationGroup(Dictionary<string, MagicOperationGroupConfiguration> groupConfigurations, Dictionary<string, OperationGroupSchema> groups, OperationGroupAttribute? group)
        {
            string key = group?.Key ?? string.Empty;

            groupConfigurations.TryGetValue(key, out MagicOperationGroupConfiguration? groupConfiguration);

            OperationGroupSchema value = new OperationGroupSchema(key, groupConfiguration?.Route);

            groups.TryAdd(key, value);

            return value;
        }

        private static OperationSchema ResolveOperation(Type operationModelType, OperationGroupSchema groupSchema)
        {
            OperationAttribute? operation = operationModelType.GetCustomAttribute<OperationAttribute>(true);

            _ = operation ?? throw new MagicOperationsException($"Operation {operationModelType.FullName} does not have {typeof(OperationAttribute).FullName}");

            OperationPropertySchema[] propertySchemas = operationModelType.GetProperties()
                                                                          .Select(OperationPropertySchemaResolver.TryResolve)
                                                                          .Where(o => o is not null)
                                                                          .ToArray()!;

            return new OperationSchema(groupSchema,
                                       operation.Renderer,
                                       operationModelType,
                                       operation.Action,
                                       operation.DisplayName ?? operation.Action?.ToUpperInvariant() ?? string.Empty,
                                       operation.HttpMethod ?? HttpMethods.Post,
                                       operation.OperationType,
                                       propertySchemas);
        }
    }
}
