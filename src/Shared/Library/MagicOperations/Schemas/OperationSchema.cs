namespace MagicOperations.Schemas
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using StringUnformatter;

    public sealed class OperationSchema
    {
        /// <summary>
        /// Operation group.
        /// </summary>
        public OperationGroupSchema Group { get; }

        /// <summary>
        /// Renderer component type. When null, default renderer will be used.
        /// </summary>
        public Type? OperationRenderer { get; }

        /// <summary>
        /// Base operation renderer type.
        /// </summary>
        public Type BaseOperationRenderer { get; }

        /// <summary>
        /// Operation model type.
        /// </summary>
        public Type OperationModelType { get; }

        /// <summary>
        /// Operation action relative to base URI and group path (if set).
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
        /// Type of response. When null, no response. Response type is assumed to be renderable even if does not have a RenderableClassAttribute.
        /// </summary>
        public Type? ResponseType { get; init; }

        /// <summary>
        /// Property schemas.
        /// </summary>
        public IReadOnlyList<RenderablePropertySchema> PropertySchemas { get; }

        private Lazy<StringTemplate> ActionTemplate { get; }

        public OperationSchema(OperationGroupSchema group,
                               Type? operationRenderer,
                               Type baseRenderer,
                               Type operationModelType,
                               string action,
                               string displayName,
                               string httpMethod,
                               Type? responseType,
                               IReadOnlyList<RenderablePropertySchema> propertySchemas)
        {
            Group = group;
            OperationModelType = operationModelType;
            OperationRenderer = operationRenderer;
            BaseOperationRenderer = baseRenderer;
            Action = action;
            DisplayName = displayName;
            HttpMethod = httpMethod;
            ResponseType = responseType;
            PropertySchemas = propertySchemas;

            ActionTemplate = new Lazy<StringTemplate>(() => StringTemplate.Parse(GetFullPath()));
        }

        public string GetFullPath()
        {
            return Path.Join(Group.Path ?? string.Empty, Action).Replace('\\', '/');
        }

        public bool MatchesPath(string path)
        {
            return ActionTemplate.Value.Matches(path);
        }

        /// <summary>
        /// Returns arguments from path, null when failed to unformat, or empty collection when successfully unformatted but no arguments were present.
        /// </summary>
        public IEnumerable<OperationUriArgument>? ExtractArguments(string path)
        {
            Dictionary<string, string>? arguments = ActionTemplate.Value.Unformat(path);

            return arguments?.Join(PropertySchemas,
                                   x => x.Key,
                                   x => x.Property.Name,
                                   (arg, schema) => new OperationUriArgument(schema, arg.Value));
        }
    }
}
