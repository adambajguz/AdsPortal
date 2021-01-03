namespace MagicOperations.Schemas
{
    public sealed class OperationUriArgument
    {
        public OperationPropertySchema Schema { get; }
        public string Value { get; }

        public OperationUriArgument(OperationPropertySchema schema, string value)
        {
            Schema = schema;
            Value = value;
        }
    }
}
