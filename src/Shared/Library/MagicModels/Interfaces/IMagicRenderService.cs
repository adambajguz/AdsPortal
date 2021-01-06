namespace MagicModels.Interfaces
{
    using Microsoft.AspNetCore.Components;

    public interface IModelRenderService
    {
        RenderFragment RenderModel(object? model, bool isWrite = false);
    }
}