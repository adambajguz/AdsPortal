namespace MagicOperations.Schemas
{
    using System;
    using System.Collections.Generic;

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
        /// Model type.
        /// </summary>
        public Type ModelType { get; }

        /// <summary>
        /// Operation action relative to base URI and group route (if set).
        /// </summary>
        public string Action { get; }

        /// <summary>
        /// Operation display name.
        /// </summary>
        public string DisplayName { get; }

        /// <summary>
        /// Http method to use.
        /// </summary>
        public string HttpMethod { get; }

        /// <summary>
        /// Property schemas.
        /// </summary>
        public IReadOnlyList<OperationPropertySchema> PropertySchemas { get; }

        /// <summary>
        /// Operation type.
        /// </summary>
        public MagicOperationTypes OperationType { get; }

        public OperationSchema(OperationGroupSchema group,
                               Type? renderer,
                               Type modelType,
                               string action,
                               string displayName,
                               string httpMethod,
                               MagicOperationTypes operationType,
                               IReadOnlyList<OperationPropertySchema> propertySchemas)
        {
            Group = group;
            Renderer = renderer;
            ModelType = modelType;
            Action = action;
            DisplayName = displayName;
            HttpMethod = httpMethod;
            OperationType = operationType;
            PropertySchemas = propertySchemas;
        }
    }
}
