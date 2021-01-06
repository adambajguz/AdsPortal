namespace MagicModels
{
    using System;
    using System.Collections.Generic;
    using MagicModels.Schemas;

    public sealed class MagicModelsConfiguration
    {
        public IReadOnlyDictionary<Type, RenderableClassSchema> RenderableTypeToSchemaMap { get; }

        public Type DefaultModelRenderer { get; }
        public Type AnyPropertyRenderer { get; }
        public IReadOnlyDictionary<Type, Type> DefaultPropertyRenderers { get; }

        public MagicModelsConfiguration(IReadOnlyDictionary<Type, RenderableClassSchema> renderableTypeToSchemaMap,
                                        IReadOnlyDictionary<Type, Type> defaultPropertyRenderers,
                                        Type defaultModelRenderer,
                                        Type anyPropertyRendererr)
        {
            RenderableTypeToSchemaMap = renderableTypeToSchemaMap;

            DefaultModelRenderer = defaultModelRenderer;
            AnyPropertyRenderer = anyPropertyRendererr;

            DefaultPropertyRenderers = defaultPropertyRenderers;
        }
    }
}
