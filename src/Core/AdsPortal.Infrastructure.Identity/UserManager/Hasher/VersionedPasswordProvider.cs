namespace AdsPortal.Infrastructure.Identity.UserManager.Hasher
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Security.Cryptography;

    internal class VersionedPasswordProvider
    {
        private readonly HasherSpecification[] _specifications =
        {
            new HasherSpecification(saltByteSize: 64, hashByteSize: 64, HashAlgorithmName.SHA512, iterations: 32769, base64Steak: "iuJ87NPOhc1a"),
        };

        private readonly HasherSpecification _lastSpecification;
        private readonly ushort _lastSpecificationIndex;

        public VersionedPasswordProvider(PasswordHasherSettings settings)
        {
            SetSpecificationFromSettings(settings);

            _lastSpecificationIndex = (ushort)(_specifications.Length - 1u);
            _lastSpecification = _specifications[_lastSpecificationIndex];
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void SetSpecificationFromSettings(PasswordHasherSettings settings)
        {
            PasswordHasherSettingsEntry[] entries = settings.Entries ?? throw new NullReferenceException("At least on entry should be defined");

            int specificationsLength = _specifications.Length;
            if (specificationsLength == 0)
                throw new ArgumentException("At least one specification should be defined");

            int entriesLength = settings.Entries?.Length ?? 0;
            if (_specifications.Length != entriesLength)
                throw new ArgumentException("Specifications and entries length should be equal");

            for (int i = 0; i < entriesLength; ++i)
            {
                HasherSpecification specification = _specifications[i];
                specification.PatchFromSettingsEntry(entries[i]);
            }
        }

        public string CreateHash(string password)
        {
            byte[] saltPepperSteak = GetSaltPepperSteak(_lastSpecification, out Span<byte> saltSpan);

            using (RNGCryptoServiceProvider csprng = new RNGCryptoServiceProvider())
                csprng.GetNonZeroBytes(saltSpan);

            byte[] hash = PBKDF2(password, saltPepperSteak, _lastSpecification);

            return new HashedPassword(saltSpan.ToArray(), hash, _lastSpecificationIndex).ToSaltedPassword();
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public bool ValidatePassword(string password, string saltedPassword)
        {
            HashedPassword correctPassword = new HashedPassword(saltedPassword, _specifications);
            HasherSpecification specification = _specifications[correctPassword.Version];

            byte[] saltPepperSteak = GetSaltPepperSteak(specification, out Span<byte> saltSpan);
            correctPassword.CopySaltTo(saltSpan);

            byte[] testHash = PBKDF2(password, saltPepperSteak, specification);

            return correctPassword.AreHashesEqual(testHash);
        }

        private static byte[] GetSaltPepperSteak(HasherSpecification specification, out Span<byte> saltSpan)
        {
            int saltByteSize = specification.SaltByteSize;
            int pepperByteSize = specification.Pepper.Length;
            int steakByteSize = specification.Steak.Length;
            int saltPepperByteSize = saltByteSize + pepperByteSize;
            int saltPepperSteakByteSize = saltPepperByteSize + steakByteSize;

            byte[] saltPepperSteak = new byte[saltPepperSteakByteSize];
            saltSpan = saltPepperSteak.AsSpan(0, saltByteSize);

            Span<byte> pepperSpan = saltPepperSteak.AsSpan(saltByteSize, pepperByteSize);
            specification.Pepper.CopyTo(pepperSpan);

            Span<byte> steakSpan = saltPepperSteak.AsSpan(saltPepperByteSize, steakByteSize);
            specification.Steak.CopyTo(steakSpan);

            return saltPepperSteak;
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static byte[] PBKDF2(string password, byte[] saltPepperSteak, HasherSpecification specification)
        {
            using (Rfc2898DeriveBytes pbkdf2 = new Rfc2898DeriveBytes(password, saltPepperSteak, specification.Iterations, specification.Algorithm))
            {
                int outputBytes = specification.HashByteSize;
                return pbkdf2.GetBytes(outputBytes);
            }
        }
    }
}
