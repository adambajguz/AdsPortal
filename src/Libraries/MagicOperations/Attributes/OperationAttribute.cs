namespace MagicOperations.Attributes
{
    using System;
    using Microsoft.AspNetCore.Http;

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public abstract class OperationAttribute : Attribute
    {
        /// <summary>
        /// Renderer component type. When null, default renderer will be used.
        /// </summary>
        public Type? Renderer { get; init; }

        /// <summary>
        /// Operation action relative to base URI and group route (if set).
        /// </summary>
        public string Action { get; }

        /// <summary>
        /// Operation display name. Defaults to uppercase route.
        /// </summary>
        public string? DisplayName { get; init; }

        /// <summary>
        /// Http method to use. Defaults to HttpMethods.Post
        /// </summary>
        public string? HttpMethod { get; init; }

        /// <summary>
        /// Operation type.
        /// </summary>
        public MagicOperationTypes OperationType { get; protected init; }

        public OperationAttribute(string defaultAction)
        {
            if(string.IsNullOrWhiteSpace(Action))
                Action = defaultAction;

            DisplayName ??= Action.ToUpperInvariant();
        }
    }

    public sealed class CreateOperationAttribute : OperationAttribute
    {
        /// <summary>
        /// Initializes an instance of <see cref="CreateOperationAttribute"/>.
        /// Default values: Action = "create"; HttpMethod = HttpMethods.Post
        /// </summary>
        public CreateOperationAttribute() : base("create")
        {
            OperationType = MagicOperationTypes.Create;
            HttpMethod ??= HttpMethods.Post;
        }
    }

    public sealed class UpdateOperationAttribute : OperationAttribute
    {
        /// <summary>
        /// Initializes an instance of <see cref="UpdateOperationAttribute"/>.
        /// Default values: Action = "update"; HttpMethod = HttpMethods.Put
        /// </summary>
        public UpdateOperationAttribute() : base("update")
        {
            OperationType = MagicOperationTypes.Update;
            HttpMethod ??= HttpMethods.Put;
        }
    }

    public sealed class DeleteOperationAttribute : OperationAttribute
    {
        /// <summary>
        /// Initializes an instance of <see cref="DeleteOperationAttribute"/>.
        /// Default values: Action = "delete/{id}"; HttpMethod = HttpMethods.Delete
        /// </summary>
        public DeleteOperationAttribute() : base("delete/{Id}")
        {
            OperationType = MagicOperationTypes.Delete;
            HttpMethod ??= HttpMethods.Delete;
        }
    }

    public sealed class DetailsOperationAttribute : OperationAttribute
    {
        /// <summary>
        /// Initializes an instance of <see cref="DetailsOperationAttribute"/>.
        /// Default values: Action = "get/{Id}"; HttpMethod = HttpMethods.Get
        /// </summary>
        public DetailsOperationAttribute() : base("get/{Id}")
        {
            OperationType = MagicOperationTypes.Details;
            HttpMethod ??= HttpMethods.Get;
        }
    }

    public sealed class GetAllOperationAttribute : OperationAttribute
    {
        /// <summary>
        /// Initializes an instance of <see cref="GetAllOperationAttribute"/>.
        /// Default values: Route = "get-all"; HttpMethod = HttpMethods.Get
        /// </summary>
        public GetAllOperationAttribute() : base("get-all")
        {
            OperationType = MagicOperationTypes.GetAll;
            HttpMethod ??= HttpMethods.Get;
        }
    }

    public sealed class GetPagedOperationAttribute : OperationAttribute
    {
        /// <summary>
        /// Initializes an instance of <see cref="GetPagedOperationAttribute"/>.
        /// Default values: Action = "get-paged"; HttpMethod = HttpMethods.Get
        /// </summary>
        public GetPagedOperationAttribute() : base("get-paged")
        {
            OperationType = MagicOperationTypes.GetPaged;
            HttpMethod ??= HttpMethods.Get;
        }
    }
}
