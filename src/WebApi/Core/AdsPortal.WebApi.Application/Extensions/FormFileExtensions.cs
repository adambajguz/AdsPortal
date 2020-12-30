namespace AdsPortal.Application.Extensions
{
    using System;
    using System.IO;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;

    public static class FormFileExtensions
    {
        public static async Task<byte[]> GetBytesAsync(this IFormFile? formFile)
        {
            if (formFile is null)
                return Array.Empty<byte>();

            using (MemoryStream? memoryStream = new MemoryStream())
            {
                await formFile.CopyToAsync(memoryStream);

                return memoryStream.ToArray();
            }
        }
    }
}
