namespace MagicOperations.Components.OperationRenderers
{
    using System;
    using System.Threading.Tasks;
    using MagicOperations.Components.OperationRenderers.Base;
    using MagicOperations.Extensions;

    public abstract class DeleteOperationRenderer<TOperation, TResponse> : OperationRenderer<TOperation, TResponse>
        where TOperation : notnull
    {
        protected bool IsDeleted { get; private set; }

        protected event EventHandler? OnDeleted;

        public async Task DeleteAsync()
        {
            if (OperationModel is null)
                return;

            try
            {
                ResponseModel = await Api.ExecuteAsync<TOperation, TResponse>(OperationModel);

                IsDeleted = true;
                OnDeleted?.Invoke(this, EventArgs.Empty);
            }
            catch (ApiException ex)
            {
                ErrorModel = ex.Message;
            }
        }
    }
}
