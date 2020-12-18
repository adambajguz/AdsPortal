namespace AdsPortal.Infrastructure.Identity.Configurations
{
    public sealed class PasswordHasherConfiguration
    {
        public PasswordHasherSettingsEntry[]? Entries { get; set; }
    }

    public sealed class PasswordHasherSettingsEntry
    {
        public int IterationsModifier { get; set; }
        public string? Pepper { get; set; }
    }
}
