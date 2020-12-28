namespace MagicOperations.Components.OperationRenderers
{
    using System.Threading.Tasks;

    public abstract class UpdateOperationRenderer : OperationRenderer
    {
        public Task SubmitAsync()
        {
            return Task.CompletedTask;
        }
    }
}
