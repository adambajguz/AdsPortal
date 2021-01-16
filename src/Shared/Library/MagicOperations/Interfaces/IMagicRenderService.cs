namespace MagicOperations.Interfaces
{
    using Microsoft.AspNetCore.Components;

    public interface IMagicRenderService
    {
        RenderFragment RenderOperationRouter(string? basePath, string? argsFallback);
        RenderFragment RenderOperation(string path, string panelPath);
        RenderFragment RenderOperation(string? basePath, string path, string panelPath);
    }
}