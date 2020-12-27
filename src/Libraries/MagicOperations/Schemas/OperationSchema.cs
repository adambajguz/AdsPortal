namespace MagicOperations.Schemas
{
    using System;

    public sealed class OperationSchema
    {
        /// <summary>
        /// Operation group.
        /// </summary>
        public OperationGroupSchema Group { get; }

        /// <summary>
        /// Renderer component type. When null, default renderer will be used.
        /// </summary>
        public Type? Renderer { get; }

        /// <summary>
        /// Operation route relative to base URI and group (if set).
        /// </summary>
        public string? Route { get; }

        /// <summary>
        /// Operation display name.
        /// </summary>
        public string DisplayName { get; }

        /// <summary>
        /// Http method to use.
        /// </summary>
        public string? HttpMethod { get; }

        public OperationSchema(OperationGroupSchema group,
                               Type? renderer,
                               string? route,
                               string displayName,
                               string? httpMethod)
        {
            Group = group;
            Renderer = renderer;
            Route = route;
            DisplayName = displayName;
            HttpMethod = httpMethod;
        }
    }
}
