namespace MagicOperations.Schemas
{
    public class OperationArgument
    {
        public OperationPropertySchema Schema { get; }
        public string Value { get; }

        public OperationArgument(OperationPropertySchema schema, string value)
        {
            Schema = schema;
            Value = value;
        }
    }
}
