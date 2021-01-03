namespace MagicOperations.Components.OperationRenderers
{
    using System;
    using System.Threading.Tasks;
    using MagicOperations.Components.OperationRenderers.Base;
    using MagicOperations.Extensions;

    public abstract class UpdateOperationRenderer<TOperation, TResponse> : SingleItemOperationRenderer<TOperation, TResponse>
        where TOperation : notnull
    {
        protected bool IsUpdated { get; private set; }

        protected event EventHandler? OnUpdated;

        public async Task UpdateAsync()
        {
            try
            {
                ResponseModel = await Api.ExecuteAsync<TOperation, TResponse>(OperationModel);

                IsUpdated = true;
                OnUpdated?.Invoke(this, EventArgs.Empty);
            }
            catch (ApiException ex)
            {
                ErrorModel = ex.Message;
            }
        }
    }
}
