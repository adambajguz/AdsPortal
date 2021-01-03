namespace MagicOperations.Interfaces
{
    using Microsoft.AspNetCore.Components;

    public interface IMagicRenderService
    {
        RenderFragment RenderModel(object? model, bool isWrite = false);
        RenderFragment RenderOperationRouter(string? basePath, string? argsFallback);
    }
}