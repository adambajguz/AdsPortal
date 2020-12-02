namespace AdsPortal.Infrastructure.Identity.UserManager.Hasher
{
    using System;
    using System.Globalization;
    using System.Security.Cryptography;
    using AdsPortal.Common.Extensions;

    internal class HashedPassword
    {
        private readonly string _rawVersion;
        private readonly string _salt;
        private readonly string _hash;

        public ushort Version { get; private set; }

        public HashedPassword(byte[] salt, byte[] hash, ushort version)
        {
            _rawVersion = version.ToString($"X{HasherSpecification.VERSION_BYTE_SIZE}");
            Version = version;

            _salt = Convert.ToBase64String(salt);
            _hash = Convert.ToBase64String(hash);
        }

        public HashedPassword(string saltedPassword, HasherSpecification[] specifications)
        {
            _rawVersion = saltedPassword.Substring(0, HasherSpecification.VERSION_BYTE_SIZE);
            Version = ushort.Parse(_rawVersion, NumberStyles.HexNumber);

            HasherSpecification specification = specifications[Version];

            int saltStrLength = Base64Extensions.GetBase64EncodedLength(specification.SaltByteSize);
            int hashIndex = saltStrLength + HasherSpecification.VERSION_BYTE_SIZE;

            _salt = saltedPassword.Substring(HasherSpecification.VERSION_BYTE_SIZE, saltStrLength);
            _hash = saltedPassword.Substring(hashIndex);
        }

        public void CopySaltTo(Span<byte> saltSpan)
        {
            byte[] bytes = Convert.FromBase64String(_salt);
            bytes.CopyTo(saltSpan);
        }

        public string ToSaltedPassword()
        {
            return _rawVersion + _salt + _hash;
        }

        public bool AreHashesEqual(byte[] hash)
        {
            byte[] dbHash = Convert.FromBase64String(_hash);

            return CryptographicOperations.FixedTimeEquals(dbHash, hash);
        }
    }
}
