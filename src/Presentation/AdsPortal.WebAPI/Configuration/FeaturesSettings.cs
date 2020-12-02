namespace AdsPortal.WebAPI.Configuration
{
    public static class FeaturesSettings
    {
        public static bool UseNewtonsoftJson { get; } = true;
        public static bool AlwaysUseExceptionHandling { get; } = true;

        //TODO: Add UseRedoc, AddFluentvalidationInfoInSwagger
    }
}
