namespace MagicOperations.Components.OperationRenderers
{
    using System.Threading.Tasks;

    public abstract class CreateOperationRenderer : OperationRenderer
    {
        public Task SubmitAsync()
        {
            return Task.CompletedTask;
        }
    }
}
