namespace MagicOperations.Internal.Schemas
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using MagicModels;
    using MagicModels.Schemas;
    using MagicOperations.Attributes;
    using MagicOperations.Builder;
    using MagicOperations.Extensions;
    using MagicOperations.Schemas;
    using Microsoft.AspNetCore.Http;

    public static class OperationSchemaResolver
    {
        public static (IReadOnlyDictionary<string, OperationGroupSchema> Groups, IReadOnlyDictionary<Type, OperationSchema> OperationTypeToSchemaMap) Resolve(IReadOnlyList<Type> operationTypes, Dictionary<string, MagicOperationGroupConfiguration> groupConfigurations, MagicModelsConfiguration modelsConfiguration)
        {
            Dictionary<string, OperationGroupSchema> groups = new();
            Dictionary<Type, OperationSchema> modelToSchemaMappings = new();

            foreach (Type operationModelType in operationTypes)
            {
                OperationGroupAttribute? groupAttribute = operationModelType.GetCustomAttribute<OperationGroupAttribute>(true);

                OperationGroupSchema groupSchema = ResolveOperationGroup(groupConfigurations, groups, groupAttribute);

                OperationSchema operationSchema = ResolveOperation(operationModelType, groupSchema, modelsConfiguration);
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

            OperationGroupSchema value = new(key, groupConfiguration?.Path, groupConfiguration?.DisplayName);
            groups.TryAdd(key, value);

            return value;
        }

        private static OperationSchema ResolveOperation(Type operationModelType, OperationGroupSchema groupSchema, MagicModelsConfiguration modelsConfiguration)
        {
            OperationAttribute? operationAttr = operationModelType.GetCustomAttribute<OperationAttribute>(true);

            _ = operationAttr ?? throw new MagicOperationsException($"Operation {operationModelType.FullName} does not have {typeof(OperationAttribute).FullName} attribute.");

            RenderableClassSchema operationModelSchema = modelsConfiguration.RenderableTypeToSchemaMap[operationModelType];
            RenderableClassSchema? responseModelSchema = operationAttr.ResponseType is null ? null : modelsConfiguration.RenderableTypeToSchemaMap[operationAttr.ResponseType];

            return new OperationSchema(groupSchema,
                                       operationAttr.OperationRenderer,
                                       operationAttr.BaseOperationRenderer,
                                       operationModelType,
                                       operationAttr.Action,
                                       operationAttr.DefaultParameters,
                                       operationAttr.DisplayName ?? $"[{operationAttr.Action.ToUpperInvariant()}] {groupSchema.DisplayName}",
                                       operationAttr.HttpMethod ?? HttpMethods.Post,
                                       operationAttr.ResponseType,
                                       operationModelSchema,
                                       responseModelSchema);
        }
    }
}
