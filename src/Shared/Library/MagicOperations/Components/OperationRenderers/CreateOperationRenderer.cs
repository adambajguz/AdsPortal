namespace MagicOperations.Components.OperationRenderers
{
    using System;
    using System.Threading.Tasks;
    using MagicOperations.Extensions;

    public abstract class CreateOperationRenderer : SingleItemOperationRenderer
    {
        protected bool IsCreated { get; private set; }

        protected event EventHandler? OnCreated;

        public async Task CreateAsync()
        {
            try
            {
                await Api.CreateAsync(Model);

                IsCreated = true;
                OnCreated?.Invoke(this, EventArgs.Empty);
            }
            catch (ApiException ex)
            {
                Errors = ex.Message;
            }
        }
    }
}
