namespace MagicModels.Components
{
    using MagicModels.Schemas;

    public interface IPropertyRenderer
    {
        object? Context { get; }
        bool IsWrite { get; }
        object Model { get; }
        RenderablePropertySchema PropertySchema { get; }
        object? Value { get; }
    }
}