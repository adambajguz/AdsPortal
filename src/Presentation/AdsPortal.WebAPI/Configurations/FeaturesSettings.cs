namespace AdsPortal.WebAPI.Configurations
{
    public static class FeaturesSettings
    {
        public static bool UseNewtonsoftJson { get; } = true;
        public static bool AlwaysUseExceptionHandling { get; } = true;

        //TODO: Add AddFluentvalidationInfoInSwagger
    }
}
