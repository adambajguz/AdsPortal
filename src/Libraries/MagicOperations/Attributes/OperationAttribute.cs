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
        /// Http method to use.
        /// </summary>
        public string? HttpMethod { get; init; }

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
            Route ??= "create";
            DisplayName ??= Route.ToUpperInvariant();
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
            Route ??= "update";
            DisplayName ??= Route.ToUpperInvariant();
            HttpMethod ??= HttpMethods.Put;
        }
    }

    public sealed class DeleteOperationAttribute : OperationAttribute
    {
        /// <summary>
        /// Initializes an instance of <see cref="DeleteOperationAttribute"/>.
        /// Default values: Route = "delete/{id:guid}"; HttpMethod = HttpMethods.Delete
        /// </summary>
        public DeleteOperationAttribute()
        {
            Route ??= "delete/{id:guid}";
            DisplayName ??= Route.ToUpperInvariant();
            HttpMethod ??= HttpMethods.Delete;
        }
    }

    public sealed class GetDetailsOperationAttribute : OperationAttribute
    {
        /// <summary>
        /// Initializes an instance of <see cref="GetDetailsOperationAttribute"/>.
        /// Default values: Route = "get/{id:guid}"; HttpMethod = HttpMethods.Get
        /// </summary>
        public GetDetailsOperationAttribute()
        {
            Route ??= "get/{id}";
            DisplayName ??= Route.ToUpperInvariant();
            HttpMethod ??= HttpMethods.Get;
        }
    }

    public sealed class GetListOperationAttribute : OperationAttribute
    {
        /// <summary>
        /// Initializes an instance of <see cref="GetListOperationAttribute"/>.
        /// Default values: Route = "get-all"; HttpMethod = HttpMethods.Get
        /// </summary>
        public GetListOperationAttribute()
        {
            Route ??= "get-all";
            DisplayName ??= Route.ToUpperInvariant();
            HttpMethod ??= HttpMethods.Get;
        }
    }

    public sealed class GetPagedListOperationAttribute : OperationAttribute
    {
        /// <summary>
        /// Initializes an instance of <see cref="GetPagedListOperationAttribute"/>.
        /// Default values: Route = "get-paged"; HttpMethod = HttpMethods.Get
        /// </summary>
        public GetPagedListOperationAttribute()
        {
            Route ??= "get-paged";
            HttpMethod ??= HttpMethods.Get;
        }
    }
}
