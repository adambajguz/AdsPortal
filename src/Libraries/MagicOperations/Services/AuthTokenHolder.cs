namespace MagicOperations.Services
{
    using System;

    public sealed class AuthTokenHolder
    {
        public string? Token { get; private set; }
        public DateTime? ValidTo { get; private set; }

        public bool HasToken => !string.IsNullOrWhiteSpace(Token);

        public void Set(string? token, DateTime? validTo)
        {
            Token = token;
            ValidTo = validTo;
        }

        public void Clear()
        {
            Token = null;
            ValidTo = null;
        }
    }
}
