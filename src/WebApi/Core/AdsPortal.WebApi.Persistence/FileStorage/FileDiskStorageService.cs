namespace AdsPortal.WebApi.Persistence.FileStorage
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Threading;
    using System.Threading.Tasks;
    using AdsPortal.Shared.Extensions.Extensions;
    using AdsPortal.WebApi.Application.Interfaces.Persistence.FileStorage;
    using AdsPortal.WebApi.Persistence.Configurations;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;

    public class FileDiskStorageService : IFileStorageService
    {
        private readonly FileDiskStorageConfiguration _fileDiskStorageConfiguration;
        private readonly string _basePath;
        private readonly ILogger _logger;

        public FileDiskStorageService(IOptions<FileDiskStorageConfiguration> fileDiskStorageConfiguration, ILogger<FileDiskStorageService> logger)
        {
            _fileDiskStorageConfiguration = fileDiskStorageConfiguration.Value;
            _logger = logger;

            _ = _fileDiskStorageConfiguration.BasePath.GetNullIfNullOrWhitespace() ?? throw new NullReferenceException($"{nameof(FileDiskStorageConfiguration)} is invalid because {nameof(FileDiskStorageConfiguration.BasePath)} is null.");
            _basePath = _fileDiskStorageConfiguration.BasePath!.Replace(Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar);
        }

        public async Task SaveFileAsync(string folder, string fileName, string contentType, byte[] data, CancellationToken cancellationToken = default)
        {
            string directory = Path.Combine(_basePath, folder);

            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);

                _logger.LogDebug("Created directory {Directory}", directory);
            }

            string filePath = Path.Combine(directory, fileName);
            await File.WriteAllBytesAsync(filePath, data, cancellationToken);

            _logger.LogDebug("Saved {Bytes} bytes to {Path}", data.Length, filePath);
        }

        public async Task SaveFileAsync(string folder, string fileName, string contentType, Stream stream, CancellationToken cancellationToken = default)
        {
            string directory = Path.Combine(_basePath, folder);

            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);

                _logger.LogDebug("Created directory {Directory}", directory);
            }

            string filePath = Path.Combine(directory, fileName);

            await using (FileStream fileStream = new(filePath, FileMode.OpenOrCreate))
            {
                stream.Position = 0;
                await stream.CopyToAsync(fileStream, cancellationToken);

                _logger.LogDebug("Saved {Bytes} bytes to {Path}", stream.Length, filePath);
            }
        }

        public Task DeleteFileAsync(string folder, string fileName, CancellationToken cancellationToken = default)
        {
            string filePath = Path.Combine(_basePath, folder, fileName);

            if (File.Exists(filePath))
            {
                File.Delete(filePath);

                _logger.LogDebug("Deleted file {Path}", filePath);
            }

            return Task.CompletedTask;
        }

        public Task<bool> FileExistsAsync(string folder, string fileName, CancellationToken cancellationToken = default)
        {
            var filePath = Path.Combine(_basePath, folder, fileName);

            return Task.FromResult(File.Exists(filePath));
        }

        public async Task<byte[]?> GetFileAsync(string folder, string fileName, CancellationToken cancellationToken = default)
        {
            string filePath = Path.Combine(_basePath, folder, fileName);

            if (File.Exists(filePath))
            {
                _logger.LogDebug("Read file {Path}", filePath);

                return await File.ReadAllBytesAsync(filePath, cancellationToken);
            }
            else
            {
                _logger.LogDebug("File {Path} not found", filePath);

                return null;
            }
        }

        public IEnumerable<string> GetDirectoryListing(string? folder = null, bool recursive = false, CancellationToken cancellationToken = default)
        {
            string directory = folder is null ? _basePath : Path.Combine(_basePath, folder);

            if (!Directory.Exists(directory))
            {
                yield break;
            }

            IEnumerable<string> listing = Directory.EnumerateFiles(directory, "*", recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly);

            foreach (string item in listing)
            {
                yield return item.TrimStart(_fileDiskStorageConfiguration.BasePath!);
            }
        }
    }
}
