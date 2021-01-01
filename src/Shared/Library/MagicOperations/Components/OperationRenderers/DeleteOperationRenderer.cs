namespace MagicOperations.Components.OperationRenderers
{
    using System.Threading.Tasks;
    using MagicOperations.Extensions;

    public abstract class DeleteOperationRenderer : SingleItemOperationRenderer
    {
        public async Task DeleteAsync()
        {
            if (Model is null)
                return;

            try
            {
                await Api.DeleteAsync(Model);
            }
            catch (ApiException ex)
            {
                Errors = ex.Message;
            }
        }
    }
}
