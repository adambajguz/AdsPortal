namespace MagicOperations.Components.OperationRenderers
{
    using System.Threading.Tasks;
    using MagicOperations.Extensions;

    public abstract class UpdateOperationRenderer : SingleItemOperationRenderer
    {
        public async Task UpdateAsync()
        {
            try
            {
                await Api.UpdateAsync(Model);
            }
            catch (ApiException ex)
            {
                Errors = ex.Message;
            }
        }
    }
}
