namespace AdsPortal.Infrastructure.Identity.UserManager.Helpers
{
    using System.Runtime.CompilerServices;
    using System.Security.Cryptography;
    using AdsPortal.Common;

    internal class PasswordHasher
    {
        private const int SALT_BYTE_SIZE = 64;
        private const int HASH_BYTE_SIZE = 64; //20 for SHA-1, 32 for SHA-256, 64 for SHA-384 and SHA-512 (or more)
        private const int PBKDF2_ITERATIONS = 52167;
        private static readonly HashAlgorithmName HASH_ALGORITHM = HashAlgorithmName.SHA512;

        public string CreateHash(string password)
        {
            byte[] salt;
            using (RNGCryptoServiceProvider csprng = new RNGCryptoServiceProvider())
            {
                salt = new byte[SALT_BYTE_SIZE];
                csprng.GetBytes(salt);
            }

            byte[] hash = PBKDF2(password, salt, PBKDF2_ITERATIONS, HASH_BYTE_SIZE, HASH_ALGORITHM);

            return new HashedPassword(salt, hash, SALT_BYTE_SIZE).ToSaltedPassword();
        }

        public bool ValidatePassword(string password, string saltedPassword)
        {
            if (password.Length < GlobalAppConfig.MIN_PASSWORD_LENGTH || password.Length > GlobalAppConfig.MAX_PASSWORD_LENGTH)
                return false;

            HashedPassword correctPassword = new HashedPassword(saltedPassword, SALT_BYTE_SIZE);
            byte[] testHash = PBKDF2(password, correctPassword.SaltToArray(), PBKDF2_ITERATIONS, HASH_BYTE_SIZE, HASH_ALGORITHM);

            return CryptographicOperations.FixedTimeEquals(correctPassword.HashToArray(), testHash);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static byte[] PBKDF2(string password, byte[] salt, int iterations, int outputBytes, HashAlgorithmName hashAlgorithm)
        {
            using (Rfc2898DeriveBytes pbkdf2 = new Rfc2898DeriveBytes(password, salt, iterations, hashAlgorithm))
                return pbkdf2.GetBytes(outputBytes);
        }
    }
}
