namespace MagicOperations.Components.OperationRenderers
{
    using System.Threading.Tasks;

    public abstract class CreateOperationRenderer : SingleItemOperationRenderer
    {
        public Task SubmitAsync()
        {
            return Task.CompletedTask;
        }
    }
}
