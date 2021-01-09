namespace AdsPortal.WebApi.Infrastructure.Identity.UserManager.Helpers
{
    using System;

    internal class HashedPassword
    {
        public int SaltByteSize { get; }
        public int SaltIndex { get; }

        public string Salt { get; private set; } = string.Empty;
        public string Hash { get; private set; } = string.Empty;

        private HashedPassword(int saltByteSize)
        {
            SaltByteSize = saltByteSize;
            SaltIndex = GetBase64EncodedLength(saltByteSize);
        }

        public HashedPassword(byte[] salt, byte[] hash, int saltByteSize) : this(saltByteSize)
        {
            Salt = Convert.ToBase64String(salt);
            Hash = Convert.ToBase64String(hash);
        }

        public HashedPassword(string salt, string hash, int saltByteSize) : this(saltByteSize)
        {
            Salt = salt;
            Hash = hash;
        }

        public HashedPassword(string saltedPassword, int saltByteSize) : this(saltByteSize)
        {
            Salt = saltedPassword.Substring(0, SaltIndex);
            Hash = saltedPassword[SaltIndex..];
        }

        private static int GetBase64EncodedLength(int byteSize)
        {
            return 4 * (int)Math.Ceiling((double)byteSize / 3);
        }

        public byte[] SaltToArray()
        {
            return Convert.FromBase64String(Salt);
        }

        public byte[] HashToArray()
        {
            return Convert.FromBase64String(Hash);
        }

        public string ToSaltedPassword()
        {
            return Salt + Hash;
        }
    }
}
