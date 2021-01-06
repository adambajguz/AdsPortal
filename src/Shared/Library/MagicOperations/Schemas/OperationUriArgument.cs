namespace MagicOperations.Schemas
{
    using MagicModels.Schemas;

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
