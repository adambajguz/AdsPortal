namespace MagicOperations.Components.OperationRenderers
{
    using System.Threading.Tasks;
    using MagicOperations.Components.OperationRenderers.Base;
    using MagicOperations.Extensions;

    public abstract class GetAllOperationRenderer : MultiItemOperationRenderer
    {
        public object? List { get; private set; }

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            await GetAllAsync();
        }

        public async Task GetAllAsync()
        {
            try
            {
                List = await Api.ExecuteAsync(OperationModel);
            }
            catch (ApiException ex)
            {
                ErrorModel = ex.Message;
            }
        }
    }
}
