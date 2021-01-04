namespace MagicOperations.Components.OperationRenderers
{
    using System.Threading.Tasks;
    using MagicOperations.Components.OperationRenderers.Base;
    using MagicOperations.Extensions;

    public abstract class GetPagedOperationRenderer<TOperation, TResponse> : OperationRenderer<TOperation, TResponse>
        where TOperation : notnull
    {
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            await GetPagedAsync();
        }

        public async Task GetPagedAsync()
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
