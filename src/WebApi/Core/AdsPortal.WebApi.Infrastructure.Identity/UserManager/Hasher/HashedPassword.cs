namespace AdsPortal.WebApi.Infrastructure.Identity.UserManager.Hasher
{
    using System;
    using System.Globalization;
    using System.Security.Cryptography;
    using AdsPortal.Shared.Extensions.Extensions;

    internal class HashedPassword
    {
        private const int VersionByteSize = sizeof(ushort); // == 2

        private readonly string _rawVersion;
        private readonly string _salt;
        private readonly string _hash;

        public ushort Version { get; private set; }

        public HashedPassword(byte[] salt, byte[] hash, ushort version)
        {
            _rawVersion = version.ToString($"X{VersionByteSize}");
            Version = version;

            _salt = Convert.ToBase64String(salt);
            _hash = Convert.ToBase64String(hash);
        }

        public HashedPassword(string saltedPassword, HasherSpecification[] specifications)
        {
            _rawVersion = saltedPassword.Substring(0, VersionByteSize);
            Version = ushort.Parse(_rawVersion, NumberStyles.HexNumber);

            HasherSpecification specification = specifications[Version];

            int saltStrLength = Base64Extensions.GetBase64EncodedLength(specification.SaltByteSize);
            int hashIndex = saltStrLength + VersionByteSize;

            _salt = saltedPassword.Substring(VersionByteSize, saltStrLength);
            _hash = saltedPassword[hashIndex..];
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
