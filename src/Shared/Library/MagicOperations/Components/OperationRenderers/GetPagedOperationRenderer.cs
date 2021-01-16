namespace MagicOperations.Components.OperationRenderers
{
    using System.Threading.Tasks;
    using MagicOperations.Extensions;

    public abstract class GetPagedOperationRenderer<TOperation, TResponse> : OperationRenderer<TOperation, TResponse>
        where TOperation : notnull
    {
        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync();

            await GetPagedAsync();
        }

        public async Task GetPagedAsync()
        {
            ResponseModel = default;

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
