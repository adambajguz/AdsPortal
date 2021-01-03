namespace MagicOperations.Components.OperationRenderers
{
    using MagicOperations.Components.OperationRenderers.Base;

    public abstract class DetailsOperationRenderer<TOperation, TResponse> : SingleItemOperationRenderer<TOperation, TResponse>
        where TOperation : notnull
    {

    }
}
