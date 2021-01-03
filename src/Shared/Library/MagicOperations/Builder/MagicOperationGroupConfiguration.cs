namespace MagicOperations.Builder
{
    public sealed class MagicOperationGroupConfiguration
    {
        /// <summary>
        /// Operation group path relative to base URI. If null, operation has no extra path.
        /// </summary>
        public string? Path { get; set; }

        /// <summary>
        /// Property display name.
        /// </summary>
        public string? DisplayName { get; set; }
    }
}
