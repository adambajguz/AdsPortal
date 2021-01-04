namespace MagicOperations.Components.OperationRenderers
{
    using MagicOperations.Components.OperationRenderers.Base;

    public abstract class DetailsOperationRenderer<TOperation, TResponse> : OperationRenderer<TOperation, TResponse>
        where TOperation : notnull
    {

    }
}
