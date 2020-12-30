namespace MagicOperations.Components.OperationRenderers
{
    using System.Threading.Tasks;

    public abstract class UpdateOperationRenderer : SingleItemOperationRenderer
    {

        public Task SubmitAsync()
        {
            return Task.CompletedTask;
        }
    }
}
