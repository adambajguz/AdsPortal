namespace MagicOperations.Components.OperationRenderers
{
    using System.Threading.Tasks;
    using MagicOperations.Extensions;

    public abstract class CreateOperationRenderer : SingleItemOperationRenderer
    {
        public async Task CreateAsync()
        {
            try
            {
                await Api.CreateAsync(Model);
            }
            catch (ApiException ex)
            {
                Errors = ex.Message;
            }
        }
    }
}
