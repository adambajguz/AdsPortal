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
        public Type? Renderer { get; }

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
        /// Type of response. When null, no response.
        /// </summary>
        public Type? ResponseType { get; init; }

        /// <summary>
        /// Property schemas.
        /// </summary>
        public IReadOnlyList<RenderablePropertySchema> PropertySchemas { get; }

        /// <summary>
        /// Operation type.
        /// </summary>
        public MagicOperationTypes OperationType { get; }

        /// <summary>
        /// Whether operation is Create or Update.
        /// </summary>
        public bool IsCreateOrUpdate => OperationType == MagicOperationTypes.Create || OperationType == MagicOperationTypes.Update;

        /// <summary>
        /// Whether operation is GetAll or GetPaged.
        /// </summary>
        public bool IsList => OperationType == MagicOperationTypes.GetAll || OperationType == MagicOperationTypes.GetPaged;

        /// <summary>
        /// Whether operation is Details.
        /// </summary>
        public bool IsDetails => OperationType == MagicOperationTypes.Details;

        /// <summary>
        /// Whether operation is Delete.
        /// </summary>
        public bool IsDelete => OperationType == MagicOperationTypes.Delete;

        private Lazy<StringTemplate> ActionTemplate { get; }

        public OperationSchema(OperationGroupSchema group,
                               Type? renderer,
                               Type operationModelType,
                               string action,
                               string displayName,
                               string httpMethod,
                               Type? responseType,
                               MagicOperationTypes operationType,
                               IReadOnlyList<RenderablePropertySchema> propertySchemas)
        {
            Group = group;
            Renderer = renderer;
            OperationModelType = operationModelType;
            Action = action;
            DisplayName = displayName;
            HttpMethod = httpMethod;
            ResponseType = responseType;
            OperationType = operationType;
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
