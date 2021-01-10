namespace AdsPortal.WebApi.Application.Interfaces.Persistence.FileStorage
{
    using System.Collections.Generic;
    using System.IO;
    using System.Threading.Tasks;

    public interface IFileStorageService
    {
        /// <summary>
        /// Saves a file from stream.
        /// </summary>
        Task SaveFileAsync(string folder, string fileName, string contentType, Stream stream);

        /// <summary>
        /// Deletes a file if exists.
        /// </summary>
        Task DeleteFileAsync(string folder, string fileName);

        /// <summary>
        /// Checks whether file exists.
        /// </summary>
        Task<bool> FileExistsAsync(string folder, string fileName);

        /// <summary>
        /// Returns file content or null if file does not exist.
        /// </summary>
        Task<byte[]?> GetFileAsync(string folder, string fileName);

        /// <summary>
        /// Returns all files in directory.
        /// </summary>
        IEnumerable<string> GetDirectoryListing(string? folder = null, bool recursive = false);
    }
}
