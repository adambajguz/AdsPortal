namespace MagicOperations.Interfaces
{
    using Microsoft.AspNetCore.Components;

    public interface IMagicRenderService
    {
        RenderFragment RenderOperation(string? path = null);
    }
}