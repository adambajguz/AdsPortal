namespace AdsPortal.WebApi.Persistence.FileStorage
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Threading.Tasks;
    using AdsPortal.Shared.Extensions.Extensions;
    using AdsPortal.WebApi.Application.Interfaces.Persistence.FileStorage;
    using AdsPortal.WebApi.Persistence.Configurations;
    using Microsoft.Extensions.Options;

    public class FileDiskStorageService : IFileStorageService
    {
        private readonly FileDiskStorageConfiguration _fileDiskStorageConfiguration;

        public FileDiskStorageService(IOptions<FileDiskStorageConfiguration> fileDiskStorageConfiguration)
        {
            _fileDiskStorageConfiguration = fileDiskStorageConfiguration.Value;

            _ = _fileDiskStorageConfiguration.BasePath.GetNullIfNullOrWhitespace() ?? throw new NullReferenceException($"{nameof(FileDiskStorageConfiguration)} is invalid because {nameof(FileDiskStorageConfiguration.BasePath)} is null.");
        }

        public async Task SaveFileAsync(string folder, string fileName, string contentType, Stream stream)
        {
            string directory = Path.Combine(_fileDiskStorageConfiguration.BasePath!, folder);

            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            string filePath = Path.Combine(directory, fileName);

            await using (FileStream fileStream = new(filePath, FileMode.OpenOrCreate))
            {
                stream.Position = 0;
                await stream.CopyToAsync(fileStream);
            }
        }

        public Task DeleteFileAsync(string folder, string fileName)
        {
            string filePath = Path.Combine(_fileDiskStorageConfiguration.BasePath!, folder, fileName);

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }

            return Task.CompletedTask;
        }

        public Task<bool> FileExistsAsync(string folder, string fileName)
        {
            var filePath = Path.Combine(_fileDiskStorageConfiguration.BasePath!, folder, fileName);

            return Task.FromResult(File.Exists(filePath));
        }

        public async Task<byte[]?> GetFileAsync(string folder, string fileName)
        {
            string filePath = Path.Combine(_fileDiskStorageConfiguration.BasePath!, folder, fileName);

            if (File.Exists(filePath))
            {
                return await File.ReadAllBytesAsync(filePath);
            }
            else
            {
                return null;
            }
        }

        public IEnumerable<string> GetDirectoryListing(string? folder = null, bool recursive = false)
        {
            string directory = folder is null ? _fileDiskStorageConfiguration.BasePath! : Path.Combine(_fileDiskStorageConfiguration.BasePath!, folder);

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
