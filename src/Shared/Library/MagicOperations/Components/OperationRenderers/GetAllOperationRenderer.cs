namespace MagicOperations.Components.OperationRenderers
{
    using System.Threading.Tasks;
    using MagicOperations.Components.OperationRenderers.Base;
    using MagicOperations.Extensions;

    public abstract class GetAllOperationRenderer<TOperation, TResponse> : MultiItemOperationRenderer<TOperation, TResponse>
        where TOperation : notnull
    {
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            await GetAllAsync();
        }

        public async Task GetAllAsync()
        {
            try
            {
                ResponseModel = await Api.ExecuteAsync<TOperation, TResponse>(OperationModel);
            }
            catch (ApiException ex)
            {
                ErrorModel = ex.Message;
            }
        }
    }
}
