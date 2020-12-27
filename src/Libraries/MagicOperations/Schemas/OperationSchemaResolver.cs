namespace MagicOperations.Schemas
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using MagicOperations.Attributes;
    using MagicOperations.Extensions;

    public static class OperationSchemaResolver
    {
        public static (IReadOnlyDictionary<string, OperationGroupSchema> Groups, IReadOnlyList<OperationSchema> Schemas) Resolve(IReadOnlyList<Type> operationTypes)
        {
            Dictionary<string, OperationGroupSchema> groups = new();
            List<OperationSchema> schemas = new();

            foreach (Type operationType in operationTypes)
            {
                OperationGroupAttribute? group = operationType.GetCustomAttribute<OperationGroupAttribute>(true);

                string route = group?.Route ?? string.Empty;
                groups.TryAdd(route, new OperationGroupSchema(route));

                OperationGroupSchema groupSchema = groups[route];

                OperationSchema operationSchema = ResolveGroup(operationType, groupSchema);
                schemas.Add(operationSchema);
                groupSchema.AddOperation(operationSchema);
            }

            return (groups, schemas);
        }

        public static OperationSchema ResolveGroup(Type operationType, OperationGroupSchema groupSchema)
        {
            OperationAttribute? operation = operationType.GetCustomAttribute<OperationAttribute>(true);

            if (operation is null)
                throw new MagicOperationsException($"Operation {operationType.FullName} does not have {typeof(OperationAttribute).FullName}");


            OperationPropertySchema[] properties = operationType.GetProperties()
                                                                .Select(OperationPropertySchemaResolver.TryResolve)
                                                                .Where(o => o is not null)
                                                                .ToArray()!;

            return new OperationSchema(groupSchema,
                                       operation.Renderer,
                                       operation.Route,
                                       operation.DisplayName ?? operation.Route?.ToUpperInvariant() ?? string.Empty,
                                       operation.HttpMethod);
        }
    }
}
