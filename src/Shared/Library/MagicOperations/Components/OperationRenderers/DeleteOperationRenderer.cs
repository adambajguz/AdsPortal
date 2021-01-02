namespace MagicOperations.Components.OperationRenderers
{
    using System;
    using System.Threading.Tasks;
    using MagicOperations.Extensions;

    public abstract class DeleteOperationRenderer : SingleItemOperationRenderer
    {
        protected bool IsDeleted { get; private set; }

        protected event EventHandler? OnDeleted;

        public async Task DeleteAsync()
        {
            if (Model is null)
                return;

            try
            {
                await Api.DeleteAsync(Model);

                IsDeleted = true;
                OnDeleted?.Invoke(this, EventArgs.Empty);
            }
            catch (ApiException ex)
            {
                Errors = ex.Message;
            }
        }
    }
}
