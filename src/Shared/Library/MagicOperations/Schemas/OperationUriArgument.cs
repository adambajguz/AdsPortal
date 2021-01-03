namespace MagicOperations.Schemas
{
    public sealed class OperationUriArgument
    {
        public RenderablePropertySchema Schema { get; }
        public string Value { get; }

        public OperationUriArgument(RenderablePropertySchema schema, string value)
        {
            Schema = schema;
            Value = value;
        }
    }
}
