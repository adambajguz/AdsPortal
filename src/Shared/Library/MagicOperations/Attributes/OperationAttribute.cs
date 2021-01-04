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
    public abstract class OperationAttribute : Attribute
    {
        /// <summary>
        /// Operation action relative to base URI and group path (if set).
        /// </summary>
        public string Action { get; init; }

        /// <summary>
        /// Operation type.
        /// </summary>
        public Type? OperationRenderer { get; protected init; }

        /// <summary>
        /// Operation type.
        /// </summary>
        public Type BaseOperationRenderer { get; protected init; }

        /// <summary>
        /// Operation display name. Defaults to uppercase action with group name.
        /// </summary>
        public string? DisplayName { get; init; }

        /// <summary>
        /// Http method to use. Defaults to HttpMethods.Post.
        /// </summary>
        public string? HttpMethod { get; init; }

        /// <summary>
        /// Type of response. When null, no response. Response type is assumed to be renderable even if does not have a RenderableClassAttribute.
        /// </summary>
        public Type? ResponseType { get; init; }

        public OperationAttribute(string defaultAction, Type baseOperationRenderer)
        {
            BaseOperationRenderer = baseOperationRenderer;

            if (OperationRenderer is not null)
            {
                if (baseOperationRenderer == OperationRenderer)
                    throw new MagicOperationsException($"Operation type cannot be equal to renderer type ({OperationRenderer.FullName}).");

                if (!baseOperationRenderer.IsSubclassOf(typeof(OperationRenderer<,>)))
                    throw new MagicOperationsException($"{baseOperationRenderer.FullName} is not a valid operation renderer type.");

                if (!OperationRenderer.IsSubclassOf(baseOperationRenderer))
                    throw new MagicOperationsException($"{OperationRenderer.FullName} is not a valid {baseOperationRenderer.FullName} operation renderer type.");
            }

            if (string.IsNullOrWhiteSpace(Action))
                Action = defaultAction;
        }
    }

    public sealed class CreateOperationAttribute : OperationAttribute
    {
        /// <summary>
        /// Initializes an instance of <see cref="CreateOperationAttribute"/>.
        /// Default values: Action = "create"; HttpMethod = HttpMethods.Post
        /// </summary>
        public CreateOperationAttribute() : base("create", typeof(CreateOperationRenderer<,>))
        {
            HttpMethod ??= HttpMethods.Post;
        }
    }

    public sealed class UpdateOperationAttribute : OperationAttribute
    {
        /// <summary>
        /// Initializes an instance of <see cref="UpdateOperationAttribute"/>.
        /// Default values: Action = "update"; HttpMethod = HttpMethods.Put
        /// </summary>
        public UpdateOperationAttribute() : base("update", typeof(UpdateOperationRenderer<,>))
        {
            HttpMethod ??= HttpMethods.Put;
        }
    }

    public sealed class DeleteOperationAttribute : OperationAttribute
    {
        /// <summary>
        /// Initializes an instance of <see cref="DeleteOperationAttribute"/>.
        /// Default values: Action = "delete/{id}"; HttpMethod = HttpMethods.Delete
        /// </summary>
        public DeleteOperationAttribute() : base("delete/{Id}", typeof(DeleteOperationRenderer<,>))
        {
            HttpMethod ??= HttpMethods.Delete;
        }
    }

    public sealed class DetailsOperationAttribute : OperationAttribute
    {
        /// <summary>
        /// Initializes an instance of <see cref="DetailsOperationAttribute"/>.
        /// Default values: Action = "get/{Id}"; HttpMethod = HttpMethods.Get
        /// </summary>
        public DetailsOperationAttribute() : base("get/{Id}", typeof(DetailsOperationRenderer<,>))
        {
            HttpMethod ??= HttpMethods.Get;
        }
    }

    public sealed class GetAllOperationAttribute : OperationAttribute
    {
        /// <summary>
        /// Initializes an instance of <see cref="GetAllOperationAttribute"/>.
        /// Default values: Route = "get-all"; HttpMethod = HttpMethods.Get
        /// </summary>
        public GetAllOperationAttribute() : base("get-all", typeof(GetAllOperationRenderer<,>))
        {
            HttpMethod ??= HttpMethods.Get;
        }
    }

    public sealed class GetPagedOperationAttribute : OperationAttribute
    {
        /// <summary>
        /// Initializes an instance of <see cref="GetPagedOperationAttribute"/>.
        /// Default values: Action = "get-paged"; HttpMethod = HttpMethods.Get
        /// </summary>
        public GetPagedOperationAttribute() : base("get-paged", typeof(GetPagedOperationRenderer<,>))
        {
            HttpMethod ??= HttpMethods.Get;
        }
    }
}
