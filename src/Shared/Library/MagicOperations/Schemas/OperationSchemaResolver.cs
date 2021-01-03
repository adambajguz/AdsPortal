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
                OperationGroupAttribute? groupAttribute = operationModelType.GetCustomAttribute<OperationGroupAttribute>(true);

                OperationGroupSchema groupSchema = ResolveOperationGroup(groupConfigurations, groups, groupAttribute);

                OperationSchema operationSchema = ResolveOperation(operationModelType, groupSchema);
                schemas.Add(operationSchema);
                modelToSchemaMappings.Add(operationModelType, operationSchema);
                groupSchema.AddOperation(operationSchema);

            }

            return (groups, schemas, modelToSchemaMappings);
        }

        private static OperationGroupSchema ResolveOperationGroup(Dictionary<string, MagicOperationGroupConfiguration> groupConfigurations, Dictionary<string, OperationGroupSchema> groups, OperationGroupAttribute? groupAttribute)
        {
            string key = groupAttribute?.Key ?? string.Empty;

            if (groups.TryGetValue(key, out OperationGroupSchema? ogs))
            {
                return ogs;
            }

            groupConfigurations.TryGetValue(key, out MagicOperationGroupConfiguration? groupConfiguration);

            OperationGroupSchema value = new OperationGroupSchema(key, groupConfiguration?.Route, groupConfiguration?.DisplayName);
            groups.TryAdd(key, value);

            return value;
        }

        private static OperationSchema ResolveOperation(Type operationModelType, OperationGroupSchema groupSchema)
        {
            OperationAttribute? operation = operationModelType.GetCustomAttribute<OperationAttribute>(true);

            _ = operation ?? throw new MagicOperationsException($"Operation {operationModelType.FullName} does not have {typeof(OperationAttribute).FullName}");

            OperationPropertySchema[] propertySchemas = operationModelType.GetProperties()
                                                                          .Select(OperationPropertySchemaResolver.TryResolve)
                                                                          .Where(x => x is not null)
                                                                          .OrderBy(x => x!.Order)
                                                                          .ToArray()!;

            return new OperationSchema(groupSchema,
                                       operation.Renderer,
                                       operationModelType,
                                       operation.Action,
                                       operation.DisplayName ?? operation.Action?.ToUpperInvariant() ?? string.Empty,
                                       operation.HttpMethod ?? HttpMethods.Post,
                                       operation.ResponseType,
                                       operation.OperationType,
                                       propertySchemas);
        }
    }
}
