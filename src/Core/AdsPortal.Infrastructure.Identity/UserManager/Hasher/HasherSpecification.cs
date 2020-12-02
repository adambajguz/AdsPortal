namespace AdsPortal.Infrastructure.Identity.UserManager.Hasher
{
    using System;
    using System.Security.Cryptography;
    using AdsPortal.Common.Extensions;

    internal sealed class HasherSpecification
    {
        private const int MINIMUM_SALT_BYTE_SIZE = 16;
        private const int MINIMUM_HASH_BYTE_SIZE = 20;

        public const int VERSION_BYTE_SIZE = sizeof(ushort); // == 2

        public int SaltByteSize { get; }
        public int HashByteSize { get; } //20 for SHA-1, 32 for SHA-256, 64 for SHA-384 and SHA-512 (or more)
        public HashAlgorithmName Algorithm { get; }
        public int Iterations { get; private set; }
        public byte[] Pepper { get; private set; } = Array.Empty<byte>();
        public byte[] Steak { get; } = Array.Empty<byte>();

        public HasherSpecification(int saltByteSize, int hashByteSize, HashAlgorithmName algorithm, int iterations, string base64Steak)
        {
            if (saltByteSize < MINIMUM_SALT_BYTE_SIZE)
                throw new ArgumentException($"SaltByteSize should be at least {MINIMUM_SALT_BYTE_SIZE} bytes");
            if (hashByteSize < MINIMUM_HASH_BYTE_SIZE)
                throw new ArgumentException($"HashByteSize should be at least {MINIMUM_HASH_BYTE_SIZE} bytes");
            if (algorithm == HashAlgorithmName.MD5 || algorithm == HashAlgorithmName.SHA1)
                throw new ArgumentException($"Algorithm should not be {algorithm}");

            SaltByteSize = saltByteSize;
            HashByteSize = hashByteSize;
            Algorithm = algorithm;
            Iterations = iterations;
            Steak = base64Steak.DecodeBase64();
        }

        public void PatchFromSettingsEntry(PasswordHasherSettingsEntry entry)
        {
            Iterations += entry.IterationsModifier;

            if (Iterations < 20000)
                throw new ArgumentException("Iterations should be at least 20000");

            Pepper = entry.Pepper?.DecodeBase64() ?? Array.Empty<byte>();
        }
    }
}
