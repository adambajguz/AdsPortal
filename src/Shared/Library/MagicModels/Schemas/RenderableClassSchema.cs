namespace MagicModels.Schemas
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using MagicModels.Attributes;

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

        /// <summary>
        /// Checks whether type is a valid renderable class.
        /// </summary>
        public static bool IsRenderableClassType(Type type)
        {
            if (type.IsAbstract || type.IsInterface || type.IsGenericType)
                return false;

            return type.IsDefined(typeof(RenderableClassAttribute));
        }
    }
}
