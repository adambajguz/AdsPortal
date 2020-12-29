namespace MagicOperations.Schemas
{
    using System;
    using System.Collections.Generic;
    using System.IO;

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

        public string GetFullRoute()
        {
            return Path.Join(Group.Route ?? string.Empty, Action).Replace('\\', '/');
        }

        public bool MatchesRoute(Uri route)
        {
            return ExtractArguments(route) is not null;
        }

        public UriTemplate.UriTemplateMatch ExtractArguments(Uri route)
        {
            string currentOprationRoute = GetFullRoute();

            UriTemplate.UriTemplate template = new UriTemplate.UriTemplate(currentOprationRoute);
            UriTemplate.UriTemplateMatch? uriTemplateMatch = template.Match(new Uri("http://localhost/"), route);

            return uriTemplateMatch;
        }
    }
}
