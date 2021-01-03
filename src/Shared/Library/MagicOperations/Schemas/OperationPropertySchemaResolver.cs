namespace MagicOperations.Schemas
{
    using System.Reflection;
    using MagicOperations.Attributes;

    public static class OperationPropertySchemaResolver
    {
        public static OperationPropertySchema? TryResolve(PropertyInfo operationProperty)
        {
            OperationPropertyIgnoreAttribute? ignoreAttribute = operationProperty.GetCustomAttribute<OperationPropertyIgnoreAttribute>(true);

            if (ignoreAttribute is not null)
                return null;

            OperationPropertyAttribute? attribute = operationProperty.GetCustomAttribute<OperationPropertyAttribute>(true);

            string displayName = attribute?.DisplayName ?? operationProperty.Name;

            return new OperationPropertySchema(operationProperty,
                                               attribute?.Renderer,
                                               displayName,
                                               attribute?.Order ?? 0);
        }
    }
}
