namespace MagicOperations.Attributes
{
    using System;
    using MagicOperations.Components;
    using MagicOperations.Components.OperationRenderers;
    using MagicOperations.Extensions;
    using Microsoft.AspNetCore.Http;

    /// <summary>
    /// Marks a class as an operation definition. By default all classes marked with operation attribute are renderable. RenderableClass attribute can be used to change operation renderer component.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public abstract class RemoteOperationAttribute : Attribute
    {
        /// <summary>
        /// Operation action relative to base URI and group path (if set).
        /// </summary>
        public string Action { get; init; }

        /// <summary>
        /// Default action parameters.
        /// </summary>
        public string[] DefaultParameters { get; init; } = Array.Empty<string>();

        /// <summary>
        /// Operation key.
        /// </summary>
        public string? Key { get; init; }

        /// <summary>
        /// Http method to use. Defaults to HttpMethods.Post.
        /// </summary>
        public string? HttpMethod { get; init; }

        /// <summary>
        /// Type of response. When null, no response. Response type is assumed to be renderable even if does not have a RenderableClassAttribute.
        /// </summary>
        public Type? ResponseType { get; init; }

        protected RemoteOperationAttribute(string key, string action)
        {
            Key = key;
            Action = action;
        }
    }
}
