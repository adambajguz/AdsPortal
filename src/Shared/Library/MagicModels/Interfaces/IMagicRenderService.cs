﻿namespace MagicModels.Interfaces
{
    using System;
    using MagicModels.Schemas;
    using Microsoft.AspNetCore.Components;

    public interface IModelRenderService
    {
        RenderFragment RenderModel(object? model, object? context = null, bool isWrite = false);
        RenderableClassSchema GetSchema(Type type);
    }
}