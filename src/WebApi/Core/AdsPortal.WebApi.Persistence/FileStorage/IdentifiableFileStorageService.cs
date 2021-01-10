namespace AdsPortal.WebApi.Persistence.FileStorage
{
    using System;
    using System.Globalization;
    using System.IO;
    using System.Threading.Tasks;
    using AdsPortal.WebApi.Application.Interfaces.Persistence.FileStorage;

    public class IdentifiableFileStorageService : IIdentifiableFileStorageService
    {
        private readonly IFileStorageService _fileStorage;

        public IdentifiableFileStorageService(IFileStorageService fileStorage)
        {
            _fileStorage = fileStorage;
        }

        public async Task SaveFileAsync(string rootFolder, Guid id, string extension, string contentType, Stream stream)
        {
            var location = BuildFileLocation(rootFolder, id, extension);
            await _fileStorage.SaveFileAsync(location.Folder, location.Name, contentType, stream);
        }

        public async Task<Guid> SaveFileAsync(string rootFolder, string extension, string contentType, Stream stream)
        {
            Guid id = Guid.NewGuid();

            var location = BuildFileLocation(rootFolder, id, extension);
            await _fileStorage.SaveFileAsync(location.Folder, location.Name, contentType, stream);

            return id;
        }

        public async Task DeleteFileAsync(string rootFolder, Guid id, string extension)
        {
            var location = BuildFileLocation(rootFolder, id, extension);
            await _fileStorage.DeleteFileAsync(location.Folder, location.Name);
        }

        public async Task<bool> FileExistsAsync(string rootFolder, Guid id, string extension)
        {
            var location = BuildFileLocation(rootFolder, id, extension);
            return await _fileStorage.FileExistsAsync(location.Folder, location.Name);
        }

        public async Task<byte[]?> GetFileAsync(string rootFolder, Guid id, string extension)
        {
            var location = BuildFileLocation(rootFolder, id, extension);
            return await _fileStorage.GetFileAsync(location.Folder, location.Name);
        }

        private static (string Folder, string Name) BuildFileLocation(string rootFolder, Guid id, string extensions)
        {
            string filename = id.ToString("D"); //"00000000-0000-0000-0000-000000000000"

            //Distribute guids into 1024 uniform buckets (0-1023).
            //https://www.codeproject.com/Articles/21508/Distributed-Caching-Using-a-Hash-Algorithm
            //https://softwareengineering.stackexchange.com/questions/286041/uniformly-distributing-guids-to-bucket-of-size-n

            string value = id.ToString("N"); //"00000000000000000000000000000000"

            uint chunk0 = uint.Parse(value.Substring(0, 8), NumberStyles.HexNumber);
            uint chunk1 = uint.Parse(value.Substring(8, 8), NumberStyles.HexNumber);
            uint chunk2 = uint.Parse(value.Substring(16, 8), NumberStyles.HexNumber);
            uint chunk3 = uint.Parse(value.Substring(24, 8), NumberStyles.HexNumber);

            ulong key = chunk0 ^ chunk1 ^ chunk2 ^ chunk3;

            int bucket = (int)(key % 1024); //Compiler should optimize this to equivalent of `key & (ulong)((1 << 10) - 1)`

            return ($"{rootFolder}{Path.DirectorySeparatorChar}{bucket:D4}", $"{filename}.{extensions.TrimStart('.')}");
        }
    }
}
