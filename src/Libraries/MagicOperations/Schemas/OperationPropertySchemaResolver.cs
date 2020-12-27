namespace MagicOperations.Schemas
{
    using System.Reflection;
    using MagicOperations.Attributes;

    public static class OperationPropertySchemaResolver
    {
        public static OperationPropertySchema? TryResolve(PropertyInfo operationProperty)
        {
            OperationPropertyAttribute? attribute = operationProperty.GetCustomAttribute<OperationPropertyAttribute>(true);

            if (attribute is null)
                return null;

            string displayName = attribute.DisplayName ?? operationProperty.Name;

            return new OperationPropertySchema(attribute.Renderer, displayName);
        }
    }
}
