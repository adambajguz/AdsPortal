namespace AdsPortal.Common.Extensions
{
    using System;
    using System.Net.Http;
    using System.Net.Http.Formatting;
    using System.Net.Http.Headers;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// Extension methods that aid in making formatted requests using <see cref="HttpClient"/>.
    /// </summary>
    public static class HttpClientExtensions
    {
        /// <summary>
        /// Sends a PATCH request as an asynchronous operation to the specified Uri with the given <paramref name="value"/> serialized
        /// as JSON.
        /// </summary>
        /// <remarks>
        /// This method uses the default instance of <see cref="JsonMediaTypeFormatter"/>.
        /// </remarks>
        /// <typeparam name="T">The type of <paramref name="value"/>.</typeparam>
        /// <param name="client">The client used to make the request.</param>
        /// <param name="requestUri">The Uri the request is sent to.</param>
        /// <param name="value">The value that will be placed in the request's entity body.</param>
        /// <returns>A task object representing the asynchronous operation.</returns>
        public static Task<HttpResponseMessage> PatchAsJsonAsync<T>(this HttpClient client, string requestUri, T value)
        {
            return client.PatchAsJsonAsync(requestUri, value, CancellationToken.None);
        }

        /// <summary>
        /// Sends a PATCH request as an asynchronous operation to the specified Uri with the given <paramref name="value"/> serialized
        /// as JSON.
        /// </summary>
        /// <remarks>
        /// This method uses the default instance of <see cref="JsonMediaTypeFormatter"/>.
        /// </remarks>
        /// <typeparam name="T">The type of <paramref name="value"/>.</typeparam>
        /// <param name="client">The client used to make the request.</param>
        /// <param name="requestUri">The Uri the request is sent to.</param>
        /// <param name="value">The value that will be placed in the request's entity body.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>A task object representing the asynchronous operation.</returns>
        public static Task<HttpResponseMessage> PatchAsJsonAsync<T>(this HttpClient client, string requestUri, T value, CancellationToken cancellationToken)
        {
            return client.PatchAsync(requestUri, value, new JsonMediaTypeFormatter(), cancellationToken);
        }

        /// <summary>
        /// Sends a PATCH request as an asynchronous operation to the specified Uri with the given <paramref name="value"/> serialized
        /// as JSON.
        /// </summary>
        /// <remarks>
        /// This method uses the default instance of <see cref="JsonMediaTypeFormatter"/>.
        /// </remarks>
        /// <typeparam name="T">The type of <paramref name="value"/>.</typeparam>
        /// <param name="client">The client used to make the request.</param>
        /// <param name="requestUri">The Uri the request is sent to.</param>
        /// <param name="value">The value that will be placed in the request's entity body.</param>
        /// <returns>A task object representing the asynchronous operation.</returns>
        public static Task<HttpResponseMessage> PatchAsJsonAsync<T>(this HttpClient client, Uri requestUri, T value)
        {
            return client.PatchAsJsonAsync(requestUri, value, CancellationToken.None);
        }

        /// <summary>
        /// Sends a PATCH request as an asynchronous operation to the specified Uri with the given <paramref name="value"/> serialized
        /// as JSON.
        /// </summary>
        /// <remarks>
        /// This method uses the default instance of <see cref="JsonMediaTypeFormatter"/>.
        /// </remarks>
        /// <typeparam name="T">The type of <paramref name="value"/>.</typeparam>
        /// <param name="client">The client used to make the request.</param>
        /// <param name="requestUri">The Uri the request is sent to.</param>
        /// <param name="value">The value that will be placed in the request's entity body.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>A task object representing the asynchronous operation.</returns>
        public static Task<HttpResponseMessage> PatchAsJsonAsync<T>(this HttpClient client, Uri requestUri, T value, CancellationToken cancellationToken)
        {
            return client.PatchAsync(requestUri, value, new JsonMediaTypeFormatter(), cancellationToken);
        }

        /// <summary>
        /// Sends a PATCH request as an asynchronous operation to the specified Uri with <paramref name="value"/>
        /// serialized using the given <paramref name="formatter"/>.
        /// </summary>
        /// <seealso cref="PatchAsync{T}(HttpClient, string, T, MediaTypeFormatter, string, CancellationToken)"/>
        /// <typeparam name="T">The type of <paramref name="value"/>.</typeparam>
        /// <param name="client">The client used to make the request.</param>
        /// <param name="requestUri">The Uri the request is sent to.</param>
        /// <param name="value">The value that will be placed in the request's entity body.</param>
        /// <param name="formatter">The formatter used to serialize the <paramref name="value"/>.</param>
        /// <returns>A task object representing the asynchronous operation.</returns>
        public static Task<HttpResponseMessage> PatchAsync<T>(this HttpClient client, string requestUri, T value, MediaTypeFormatter formatter)
        {
            return client.PatchAsync(requestUri, value, formatter, CancellationToken.None);
        }

        /// <summary>
        /// Sends a PATCH request as an asynchronous operation to the specified Uri with <paramref name="value"/>
        /// serialized using the given <paramref name="formatter"/>.
        /// </summary>
        /// <seealso cref="PatchAsync{T}(HttpClient, string, T, MediaTypeFormatter, string, CancellationToken)"/>
        /// <typeparam name="T">The type of <paramref name="value"/>.</typeparam>
        /// <param name="client">The client used to make the request.</param>
        /// <param name="requestUri">The Uri the request is sent to.</param>
        /// <param name="value">The value that will be placed in the request's entity body.</param>
        /// <param name="formatter">The formatter used to serialize the <paramref name="value"/>.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>A task object representing the asynchronous operation.</returns>
        public static Task<HttpResponseMessage> PatchAsync<T>(this HttpClient client, string requestUri, T value, MediaTypeFormatter formatter, CancellationToken cancellationToken)
        {
            return client.PatchAsync(requestUri, value, formatter, mediaType: null!, cancellationToken: cancellationToken);
        }

        /// <summary>
        /// Sends a PATCH request as an asynchronous operation to the specified Uri with <paramref name="value"/>
        /// serialized using the given <paramref name="formatter"/>.
        /// </summary>
        /// <typeparam name="T">The type of <paramref name="value"/>.</typeparam>
        /// <param name="client">The client used to make the request.</param>
        /// <param name="requestUri">The Uri the request is sent to.</param>
        /// <param name="value">The value that will be placed in the request's entity body.</param>
        /// <param name="formatter">The formatter used to serialize the <paramref name="value"/>.</param>
        /// <param name="mediaType">The authoritative value of the request's content's Content-Type header. Can be <c>null</c> in which case the
        /// <paramref name="formatter">formatter's</paramref> default content type will be used.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>A task object representing the asynchronous operation.</returns>
        public static Task<HttpResponseMessage> PatchAsync<T>(this HttpClient client, string requestUri, T value, MediaTypeFormatter formatter, MediaTypeHeaderValue mediaType, CancellationToken cancellationToken)
        {
            if (client == null)
            {
                throw new ArgumentNullException("client");
            }

            var content = new ObjectContent<T>(value, formatter, mediaType);

            return client.PatchAsync(requestUri, content, cancellationToken);
        }

        /// <summary>
        /// Sends a PATCH request as an asynchronous operation to the specified Uri with <paramref name="value"/>
        /// serialized using the given <paramref name="formatter"/>.
        /// </summary>
        /// <seealso cref="PatchAsync{T}(HttpClient, string, T, MediaTypeFormatter, string, CancellationToken)"/>
        /// <typeparam name="T">The type of <paramref name="value"/>.</typeparam>
        /// <param name="client">The client used to make the request.</param>
        /// <param name="requestUri">The Uri the request is sent to.</param>
        /// <param name="value">The value that will be placed in the request's entity body.</param>
        /// <param name="formatter">The formatter used to serialize the <paramref name="value"/>.</param>
        /// <returns>A task object representing the asynchronous operation.</returns>
        public static Task<HttpResponseMessage> PatchAsync<T>(this HttpClient client, Uri requestUri, T value, MediaTypeFormatter formatter)
        {
            return client.PatchAsync(requestUri, value, formatter, CancellationToken.None);
        }

        /// <summary>
        /// Sends a PATCH request as an asynchronous operation to the specified Uri with <paramref name="value"/>
        /// serialized using the given <paramref name="formatter"/>.
        /// </summary>
        /// <seealso cref="PatchAsync{T}(HttpClient, string, T, MediaTypeFormatter, string, CancellationToken)"/>
        /// <typeparam name="T">The type of <paramref name="value"/>.</typeparam>
        /// <param name="client">The client used to make the request.</param>
        /// <param name="requestUri">The Uri the request is sent to.</param>
        /// <param name="value">The value that will be placed in the request's entity body.</param>
        /// <param name="formatter">The formatter used to serialize the <paramref name="value"/>.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>A task object representing the asynchronous operation.</returns>
        public static Task<HttpResponseMessage> PatchAsync<T>(this HttpClient client, Uri requestUri, T value, MediaTypeFormatter formatter, CancellationToken cancellationToken)
        {
            return client.PatchAsync(requestUri, value, formatter, mediaType: null!, cancellationToken: cancellationToken);
        }

        /// <summary>
        /// Sends a PATCH request as an asynchronous operation to the specified Uri with <paramref name="value"/>
        /// serialized using the given <paramref name="formatter"/>.
        /// </summary>
        /// <typeparam name="T">The type of <paramref name="value"/>.</typeparam>
        /// <param name="client">The client used to make the request.</param>
        /// <param name="requestUri">The Uri the request is sent to.</param>
        /// <param name="value">The value that will be placed in the request's entity body.</param>
        /// <param name="formatter">The formatter used to serialize the <paramref name="value"/>.</param>
        /// <param name="mediaType">The authoritative value of the request's content's Content-Type header. Can be <c>null</c> in which case the
        /// <paramref name="formatter">formatter's</paramref> default content type will be used.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>A task object representing the asynchronous operation.</returns>
        public static Task<HttpResponseMessage> PatchAsync<T>(this HttpClient client, Uri requestUri, T value, MediaTypeFormatter formatter, MediaTypeHeaderValue mediaType, CancellationToken cancellationToken)
        {
            if (client == null)
            {
                throw new ArgumentNullException("client");
            }

            var content = new ObjectContent<T>(value, formatter, mediaType);

            return client.PatchAsync(requestUri, content, cancellationToken);
        }
    }
}
