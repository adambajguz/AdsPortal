namespace MagicOperations.Interfaces
{
    using Microsoft.AspNetCore.Components;

    public interface IMagicOperationsService
    {
        RenderFragment RenderModel(object? model);
        RenderFragment RenderOperationRouter(string? basePath, string? argsFallback);
    }
}