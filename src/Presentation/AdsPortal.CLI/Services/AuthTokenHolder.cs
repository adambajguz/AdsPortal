namespace AdsPortal.CLI.Services
{
    public sealed class AuthTokenHolder
    {
        public string? Token { get; set; }
        public bool HasToken => !string.IsNullOrWhiteSpace(Token);
    }
}
