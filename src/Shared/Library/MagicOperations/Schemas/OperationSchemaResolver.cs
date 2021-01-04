namespace MagicOperations.Schemas
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using MagicOperations.Attributes;
    using MagicOperations.Builder;
    using MagicOperations.Extensions;
    using Microsoft.AspNetCore.Http;

    public static class OperationSchemaResolver
    {
        public static (IReadOnlyDictionary<string, OperationGroupSchema> Groups, IReadOnlyDictionary<Type, OperationSchema> OperationTypeToSchemaMap) Resolve(IReadOnlyList<Type> operationTypes, Dictionary<string, MagicOperationGroupConfiguration> groupConfigurations)
        {
            Dictionary<string, OperationGroupSchema> groups = new();
            Dictionary<Type, OperationSchema> modelToSchemaMappings = new();

            foreach (Type operationModelType in operationTypes)
            {
                OperationGroupAttribute? groupAttribute = operationModelType.GetCustomAttribute<OperationGroupAttribute>(true);

                OperationGroupSchema groupSchema = ResolveOperationGroup(groupConfigurations, groups, groupAttribute);

                OperationSchema operationSchema = ResolveOperation(operationModelType, groupSchema);
                modelToSchemaMappings.Add(operationModelType, operationSchema);
                groupSchema.AddOperation(operationSchema);

            }

            return (groups, modelToSchemaMappings);
        }

        private static OperationGroupSchema ResolveOperationGroup(Dictionary<string, MagicOperationGroupConfiguration> groupConfigurations, Dictionary<string, OperationGroupSchema> groups, OperationGroupAttribute? groupAttribute)
        {
            string key = groupAttribute?.Key ?? string.Empty;

            if (groups.TryGetValue(key, out OperationGroupSchema? ogs))
            {
                return ogs;
            }

            groupConfigurations.TryGetValue(key, out MagicOperationGroupConfiguration? groupConfiguration);

            OperationGroupSchema value = new OperationGroupSchema(key, groupConfiguration?.Path, groupConfiguration?.DisplayName);
            groups.TryAdd(key, value);

            return value;
        }

        private static OperationSchema ResolveOperation(Type operationModelType, OperationGroupSchema groupSchema)
        {
            OperationAttribute? operationAttr = operationModelType.GetCustomAttribute<OperationAttribute>(true);

            _ = operationAttr ?? throw new MagicOperationsException($"Operation {operationModelType.FullName} does not have {typeof(OperationAttribute).FullName} attribute.");

            Type? responseType = operationAttr.ResponseType;
            if (responseType?.IsGenericType ?? false)
            {
                foreach (var paramType in responseType.GetGenericArguments())
                {
                    if (!KnownTypesHelpers.IsRenderableClassType(paramType))
                    {
                        throw new MagicOperationsException($"Operation response type {responseType} is not a renderable type.");
                    }
                }
            }
            else if (responseType is not null && !KnownTypesHelpers.IsRenderableClassType(responseType))
            {
                throw new MagicOperationsException($"Operation response type {responseType} is not a renderable type.");
            }

            RenderablePropertySchema[] propertySchemas = operationModelType.GetProperties()
                                                                          .Select(RenderablePropertySchemaResolver.TryResolve)
                                                                          .Where(x => x is not null)
                                                                          .OrderBy(x => x!.Order)
                                                                          .ToArray()!;

            return new OperationSchema(groupSchema,
                                       operationAttr.OperationRenderer,
                                       operationAttr.BaseOperationRenderer,
                                       operationModelType,
                                       operationAttr.Action,
                                       operationAttr.DisplayName ?? $"[{operationAttr.Action.ToUpperInvariant()}] {groupSchema.DisplayName}",
                                       operationAttr.HttpMethod ?? HttpMethods.Post,
                                       responseType,
                                       propertySchemas);
        }
    }
}
