﻿namespace AdsPortal.WebApi.Application.Interfaces.Persistence.FileStorage
{
    using System;
    using System.IO;
    using System.Threading;
    using System.Threading.Tasks;

    public interface IIdentifiableFileStorageService
    {
        /// <summary>
        /// Saves a file from byte array with specified id.
        /// </summary>
        Task SaveFileAsync(string rootFolder, Guid id, string extension, string contentType, byte[] data, CancellationToken cancellationToken = default);

        /// <summary>
        /// Saves a file from byte array and returns file id.
        /// </summary>
        Task<Guid> SaveFileAsync(string rootFolder, string extension, string contentType, byte[] data, CancellationToken cancellationToken = default);

        /// <summary>
        /// Saves a file from stream with specified id.
        /// </summary>
        Task SaveFileAsync(string rootFolder, Guid id, string extension, string contentType, Stream stream, CancellationToken cancellationToken = default);

        /// <summary>
        /// Saves a file from stream and returns file id.
        /// </summary>
        Task<Guid> SaveFileAsync(string rootFolder, string extension, string contentType, Stream stream, CancellationToken cancellationToken = default);

        /// <summary>
        /// Deletes a file if exists.
        /// </summary>
        Task DeleteFileAsync(string rootFolder, Guid id, string extension, CancellationToken cancellationToken = default);

        /// <summary>
        /// Checks whether file exists.
        /// </summary>
        Task<bool> FileExistsAsync(string rootFolder, Guid id, string extension, CancellationToken cancellationToken = default);

        /// <summary>
        /// Returns file content or null if file does not exist.
        /// </summary>
        Task<byte[]?> GetFileAsync(string rootFolder, Guid id, string extension, CancellationToken cancellationToken = default);
    }
}
