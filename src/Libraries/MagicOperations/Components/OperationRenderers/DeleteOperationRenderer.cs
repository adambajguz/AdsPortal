namespace MagicOperations.Components.OperationRenderers
{
    using System.Threading;
    using System.Threading.Tasks;

    public abstract class DeleteOperationRenderer : OperationRenderer
    {
        public async Task SubmitAsync(CancellationToken cancellationToken = default)
        {
            if (Model is null)
                return;

            await MagicApi.ExecuteAsync(Model, cancellationToken);
        }
    }
}
