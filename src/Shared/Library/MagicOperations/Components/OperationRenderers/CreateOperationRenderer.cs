namespace MagicOperations.Components.OperationRenderers
{
    using System;
    using System.Threading.Tasks;
    using MagicOperations.Components.OperationRenderers.Base;
    using MagicOperations.Extensions;

    public abstract class CreateOperationRenderer<TOperation, TResponse> : SingleItemOperationRenderer<TOperation, TResponse>
        where TOperation : notnull
    {
        protected bool IsCreated { get; private set; }

        protected event EventHandler? OnCreated;

        public async Task CreateAsync()
        {
            try
            {
                ResponseModel = await Api.ExecuteAsync<TOperation, TResponse>(OperationModel);

                IsCreated = true;
                OnCreated?.Invoke(this, EventArgs.Empty);
            }
            catch (ApiException ex)
            {
                ErrorModel = ex.Message;
            }
        }
    }
}
