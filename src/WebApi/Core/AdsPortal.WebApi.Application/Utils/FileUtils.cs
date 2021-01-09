namespace AdsPortal.WebApi.Application.Utils
{
    using System;
    using System.Globalization;
    using AdsPortal.Shared.Extensions.Utils;
    using CSharpVitamins;

    public static class FileUtils
    {
        public static string GenerateEmptyFileName(bool addExtension = true, string extension = "txt")
        {
            string timestamp = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture);

            if (addExtension)
            {
                return $"empty-{timestamp}-{ShortGuid.NewGuid()}.{extension}";
            }

            return $"empty-{timestamp}-{ShortGuid.NewGuid()}";
        }

        public static string GetChecksumFileContent(byte[] hash, string filename)
        {
            return string.Concat(HexUtils.ToHexString(hash), " *", filename);
        }

        public static string GetChecksumFileContent(string hash, string filename)
        {
            return string.Concat(hash, " *", filename);
        }
    }
}
