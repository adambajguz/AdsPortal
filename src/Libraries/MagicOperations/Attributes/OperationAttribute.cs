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
        /// Operation route relative to base URI and group (if set).
        /// </summary>
        public string? Route { get; init; }

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

        public OperationAttribute()
        {
            DisplayName ??= Route?.ToUpperInvariant();
        }
    }

    public sealed class CreateOperationAttribute : OperationAttribute
    {
        /// <summary>
        /// Initializes an instance of <see cref="CreateOperationAttribute"/>.
        /// Default values: Route = "create"; HttpMethod = HttpMethods.Post
        /// </summary>
        public CreateOperationAttribute()
        {
            OperationType = MagicOperationTypes.Create;
            Route ??= "create";
            HttpMethod ??= HttpMethods.Post;
        }
    }

    public sealed class UpdateOperationAttribute : OperationAttribute
    {
        /// <summary>
        /// Initializes an instance of <see cref="UpdateOperationAttribute"/>.
        /// Default values: Route = "update"; HttpMethod = HttpMethods.Put
        /// </summary>
        public UpdateOperationAttribute()
        {
            OperationType = MagicOperationTypes.Update;
            Route ??= "update";
            HttpMethod ??= HttpMethods.Put;
        }
    }

    public sealed class DeleteOperationAttribute : OperationAttribute
    {
        /// <summary>
        /// Initializes an instance of <see cref="DeleteOperationAttribute"/>.
        /// Default values: Route = "delete/{id}"; HttpMethod = HttpMethods.Delete
        /// </summary>
        public DeleteOperationAttribute()
        {
            OperationType = MagicOperationTypes.Delete;
            Route ??= "delete/{Id}";
            HttpMethod ??= HttpMethods.Delete;
        }
    }

    public sealed class DetailsOperationAttribute : OperationAttribute
    {
        /// <summary>
        /// Initializes an instance of <see cref="DetailsOperationAttribute"/>.
        /// Default values: Route = "get/{Id}"; HttpMethod = HttpMethods.Get
        /// </summary>
        public DetailsOperationAttribute()
        {
            OperationType = MagicOperationTypes.Details;
            Route ??= "get/{Id}";
            HttpMethod ??= HttpMethods.Get;
        }
    }

    public sealed class GetAllOperationAttribute : OperationAttribute
    {
        /// <summary>
        /// Initializes an instance of <see cref="GetAllOperationAttribute"/>.
        /// Default values: Route = "get-all"; HttpMethod = HttpMethods.Get
        /// </summary>
        public GetAllOperationAttribute()
        {
            OperationType = MagicOperationTypes.GetAll;
            Route ??= "get-all";
            HttpMethod ??= HttpMethods.Get;
        }
    }

    public sealed class GetPagedOperationAttribute : OperationAttribute
    {
        /// <summary>
        /// Initializes an instance of <see cref="GetPagedOperationAttribute"/>.
        /// Default values: Route = "get-paged"; HttpMethod = HttpMethods.Get
        /// </summary>
        public GetPagedOperationAttribute()
        {
            OperationType = MagicOperationTypes.GetPaged;
            Route ??= "get-paged";
            HttpMethod ??= HttpMethods.Get;
        }
    }
}
