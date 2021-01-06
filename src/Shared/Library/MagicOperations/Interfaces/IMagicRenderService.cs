namespace MagicOperations.Interfaces
{
    using Microsoft.AspNetCore.Components;

    public interface IMagicRenderService
    {
        RenderFragment RenderOperationRouter(string? basePath, string? argsFallback);
    }
}