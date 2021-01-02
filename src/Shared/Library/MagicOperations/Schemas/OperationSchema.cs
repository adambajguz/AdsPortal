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
        /// Type of response. When null, no response.
        /// </summary>
        public Type? ResponseType { get; init; }

        /// <summary>
        /// Property schemas.
        /// </summary>
        public IReadOnlyList<OperationPropertySchema> PropertySchemas { get; }

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

        private Lazy<StringTemplate> RouteTemplate { get; }

        public OperationSchema(OperationGroupSchema group,
                               Type? renderer,
                               Type modelType,
                               string action,
                               string displayName,
                               string httpMethod,
                               Type? responseType,
                               MagicOperationTypes operationType,
                               IReadOnlyList<OperationPropertySchema> propertySchemas)
        {
            Group = group;
            Renderer = renderer;
            ModelType = modelType;
            Action = action;
            DisplayName = displayName;
            HttpMethod = httpMethod;
            ResponseType = responseType;
            OperationType = operationType;
            PropertySchemas = propertySchemas;

            RouteTemplate = new Lazy<StringTemplate>(() => StringTemplate.Parse(GetFullRoute()));
        }

        public string GetFullRoute()
        {
            return Path.Join(Group.Route ?? string.Empty, Action).Replace('\\', '/');
        }

        public bool MatchesRoute(string route)
        {
            return RouteTemplate.Value.Matches(route);
        }

        public IEnumerable<OperationArgument>? ExtractArguments(string route)
        {
            Dictionary<string, string>? arguments = RouteTemplate.Value.Unformat(route);

            return arguments?.Join(PropertySchemas,
                                   x => x.Key,
                                   x => x.Property.Name,
                                   (arg, schema) => new OperationArgument(schema, arg.Value));
        }
    }
}
