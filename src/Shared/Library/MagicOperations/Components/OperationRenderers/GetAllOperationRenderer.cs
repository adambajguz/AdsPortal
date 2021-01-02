namespace MagicOperations.Components.OperationRenderers
{
    using System.Threading.Tasks;
    using MagicOperations.Extensions;

    public abstract class GetAllOperationRenderer : OperationRenderer
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
                List = await Api.GetList(Model);
            }
            catch (ApiException ex)
            {
                Errors = ex.Message;
            }
        }
    }
}
