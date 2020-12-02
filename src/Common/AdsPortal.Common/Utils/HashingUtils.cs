namespace AdsPortal.Common.Utils
{
    using System.Security.Cryptography;

    public static class HashingUtils
    {
        public static byte[] GetSHA512(byte[] data)
        {
            using (SHA512 hasher = SHA512.Create())
            {
                byte[] hash = hasher.ComputeHash(data);

                return hash;
            }
        }

        public static string GetSHA512Hex(byte[] data)
        {
            using (SHA512 hasher = SHA512.Create())
            {
                byte[] hash = hasher.ComputeHash(data);

                return HexUtils.ToHexString(hash);
            }
        }
    }
}
