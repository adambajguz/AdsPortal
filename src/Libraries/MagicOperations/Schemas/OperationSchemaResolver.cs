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
        public static (IReadOnlyDictionary<string, OperationGroupSchema> Groups, IReadOnlyList<OperationSchema> Schemas, IReadOnlyDictionary<Type, OperationSchema> ModelToSchemaMappings) Resolve(IReadOnlyList<Type> operationTypes)
        {
            Dictionary<string, OperationGroupSchema> groups = new();
            List<OperationSchema> schemas = new();
            Dictionary<Type, OperationSchema> modelToSchemaMappings = new();

            foreach (Type operationModelType in operationTypes)
            {
                OperationGroupAttribute? group = operationModelType.GetCustomAttribute<OperationGroupAttribute>(true);

                string route = group?.Route ?? string.Empty;
                groups.TryAdd(route, new OperationGroupSchema(route));

                OperationGroupSchema groupSchema = groups[route];

                OperationSchema operationSchema = ResolveGroup(operationModelType, groupSchema);
                schemas.Add(operationSchema);
                groupSchema.AddOperation(operationSchema);
                modelToSchemaMappings.Add(operationModelType, operationSchema);
            }

            return (groups, schemas, modelToSchemaMappings);
        }

        public static OperationSchema ResolveGroup(Type operationModelType, OperationGroupSchema groupSchema)
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
                                       operation.Route,
                                       operation.DisplayName ?? operation.Route?.ToUpperInvariant() ?? string.Empty,
                                       operation.HttpMethod ?? HttpMethods.Post,
                                       operation.OperationType,
                                       propertySchemas);
        }
    }
}
