namespace MagicOperations.Schemas
{
    using System;
    using System.Collections.Generic;

    public sealed class RenderableClassSchema
    {
        /// <summary>
        /// Renderer component type. When null, default renderer will be used.
        /// </summary>
        public Type? Renderer { get; }

        /// <summary>
        /// Model type.
        /// </summary>
        public Type ClassType { get; }

        /// <summary>
        /// Property schemas.
        /// </summary>
        public IReadOnlyList<RenderablePropertySchema> PropertySchemas { get; }

        public RenderableClassSchema(Type? renderer,
                                     Type classType,
                                     IReadOnlyList<RenderablePropertySchema> propertySchemas)
        {
            Renderer = renderer;
            ClassType = classType;
            PropertySchemas = propertySchemas;
        }
    }
}
