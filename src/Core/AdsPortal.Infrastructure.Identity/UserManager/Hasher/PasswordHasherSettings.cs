namespace AdsPortal.Infrastructure.Identity.UserManager.Hasher
{
    public sealed class PasswordHasherSettings
    {
        public PasswordHasherSettingsEntry[]? Entries { get; set; }
    }

    public sealed class PasswordHasherSettingsEntry
    {
        public int IterationsModifier { get; set; }
        public string? Pepper { get; set; }
    }
}
