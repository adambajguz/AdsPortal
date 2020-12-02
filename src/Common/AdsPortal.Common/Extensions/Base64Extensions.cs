namespace AdsPortal.Common.Extensions
{
    using System;
    using System.Threading.Tasks;

    public static class Base64Extensions
    {
        public static int GetBase64EncodedLength(int byteSize)
        {
            return 4 * (int)Math.Ceiling((double)byteSize / 3);
        }

        public static int GetBase64EncodedLength(this byte[] data)
        {
            return 4 * (int)Math.Ceiling((double)data.Length / 3);
        }

        public static async Task<string> EncodeBase64BinaryAsync(this byte[] data)
        {
            if (data is null)
                throw new ArgumentNullException(nameof(data));

            string encodedData = await Task.Run(() => Convert.ToBase64String(data))
                                           .ConfigureAwait(false);

            return encodedData;
        }

        public static async Task<byte[]> DecodeBase64BinaryAsync(this byte[] base64Data)
        {
            if (base64Data is null)
                throw new ArgumentNullException(nameof(base64Data));

            byte[] decodedData = await Task.Run(() =>
            {
                string str = System.Text.Encoding.UTF8.GetString(base64Data);
                return Convert.FromBase64String(str);

            }).ConfigureAwait(false);

            return decodedData;
        }

        public static async Task<byte[]> DecodeBase64Async(this string base64Data)
        {
            if (string.IsNullOrWhiteSpace(base64Data))
                throw new ArgumentException("Cannot be null or whitespace", nameof(base64Data));

            byte[] decodedData = await Task.Run(() =>
            {
                return Convert.FromBase64String(base64Data);

            }).ConfigureAwait(false);

            return decodedData;
        }

        public static string EncodeBase64Binary(this byte[] data)
        {
            if (data is null)
                throw new ArgumentNullException(nameof(data));

            return Convert.ToBase64String(data);
        }

        public static byte[] DecodeBase64Binary(this byte[] base64Data)
        {
            if (base64Data is null)
                throw new ArgumentNullException(nameof(base64Data));

            string str = System.Text.Encoding.UTF8.GetString(base64Data);

            return Convert.FromBase64String(str);
        }

        public static byte[] DecodeBase64(this string base64Data)
        {
            if (string.IsNullOrWhiteSpace(base64Data))
                throw new ArgumentException("Cannot be null or whitespace", nameof(base64Data));

            return Convert.FromBase64String(base64Data);
        }
    }
}
