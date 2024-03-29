﻿namespace MagicOperations.Schemas
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using System.Text.RegularExpressions;
    using MagicModels.Schemas;
    using MagicOperations.Extensions;
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
        /// Default action parameters.
        /// </summary>
        public string[] DefaultParameters { get; init; }

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

        public RenderableClassSchema OperationModelSchema { get; }

        public RenderableClassSchema? ResponseModelSchema { get; }

        private Lazy<StringTemplate> ActionTemplate { get; }

        public OperationSchema(OperationGroupSchema group,
                               Type? operationRenderer,
                               Type baseRenderer,
                               Type operationModelType,
                               string action,
                               string[] defaultParameters,
                               string displayName,
                               string httpMethod,
                               Type? responseType,
                               RenderableClassSchema operationModelSchema,
                               RenderableClassSchema? responseModelSchema)
        {
            Group = group;
            OperationModelType = operationModelType;
            OperationRenderer = operationRenderer;
            BaseOperationRenderer = baseRenderer;
            Action = action;
            DefaultParameters = defaultParameters;
            DisplayName = displayName;
            HttpMethod = httpMethod;
            ResponseType = responseType;
            OperationModelSchema = operationModelSchema;
            ResponseModelSchema = responseModelSchema;

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

            return arguments?.Join(OperationModelSchema.PropertySchemas,
                                   x => x.Key,
                                   x => x.Property.Name,
                                   (arg, schema) => new OperationUriArgument(schema, arg.Value));
        }

        private static readonly Regex _regex = new(@"(?<=\{)[^}{]*(?=\})", RegexOptions.IgnoreCase);

        public string GetFullPathFromModel(object model)
        {
            MatchCollection matches = _regex.Matches(Action);

            List<string> tokens = matches.Cast<Match>()
                                         .Select(m => m.Value)
                                         .Distinct()
                                         .ToList();

            string patchedRoute = Action;

            if (tokens.Count > OperationModelSchema.PropertySchemas.Count) //TODO: what if more properties in operation rendering?
            {
                throw new MagicOperationsException("All action arguments must be bound from model properties."); //TODO: add validation on app start
            }

            foreach (string t in tokens)
            {
                PropertyInfo propertyInfo = OperationModelSchema.PropertySchemas.Where(x => string.Equals(t, x.Property.Name, StringComparison.Ordinal))
                                                                                .FirstOrDefault()?.Property ??
                                            throw new MagicOperationsException($"Cannot bind argument '{t}'.");

                string value = propertyInfo.GetValue(model)?.ToString() ?? string.Empty;

                patchedRoute = patchedRoute.Replace($"{{{t}}}", value);
            }

            return Path.Join(Group.Path ?? string.Empty, patchedRoute).Replace('\\', '/');
        }

        public string GetFullPathFromDictionary(IReadOnlyDictionary<string, string>? arguments = null)
        {
            MatchCollection matches = _regex.Matches(Action);

            List<string> tokens = matches.Cast<Match>()
                                         .Select(m => m.Value)
                                         .Distinct()
                                         .ToList();

            string patchedRoute = Action;


            if (tokens.Count > OperationModelSchema.PropertySchemas.Count) //TODO: what if more properties in operation rendering?
            {
                throw new MagicOperationsException("All action arguments must be bound from dictionary."); //TODO: add validation on app start
            }

            if (tokens.Count > (arguments?.Count ?? 0))
            {
                throw new MagicOperationsException("All action arguments must be bound from dictionary."); //TODO: add validation on app start
            }

            foreach (string t in tokens)
            {
                string value = arguments!.GetValueOrDefault(t) ?? t;

                patchedRoute = patchedRoute.Replace($"{{{t}}}", value);
            }

            return Path.Join(Group.Path ?? string.Empty, patchedRoute).Replace('\\', '/');
        }

        public string GetDefaultPath()
        {
            StringBuilder defaultPath = new();

            for (int i = 0, param = 0; i < ActionTemplate.Value.Parts.Count; ++i)
            {
                StringTemplatePart part = ActionTemplate.Value.Parts[i];

                string value = part.IsParameter && param < DefaultParameters.Length ?
                                   DefaultParameters[param++] : part.Value;

                defaultPath.Append(value);
            }

            return defaultPath.ToString();
        }
    }
}
