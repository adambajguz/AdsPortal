namespace MagicOperations.Components.OperationRenderers
{
    using System.Threading.Tasks;
    using MagicOperations.Extensions;

    public abstract class DetailsOperationRenderer<TOperation, TResponse> : OperationRenderer<TOperation, TResponse>
        where TOperation : notnull
    {
        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync();

            StateHasChanged();
            await GetAsync();
        }

        public async Task GetAsync()
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
