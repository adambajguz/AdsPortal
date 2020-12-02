namespace AdsPortal.Common
{
    public static partial class GlobalAppConfig
    {
        public const bool DEV_MODE_SW = true;
        public const bool USE_COMPILATION_CONFIGURATION_TO_DETERMINE_ENVIRONMENT = true;
        public static bool IsDevMode
        {
            get
            {
#pragma warning disable CS0162 // Unreachable code detected
                if (USE_COMPILATION_CONFIGURATION_TO_DETERMINE_ENVIRONMENT)
                {
#if DEBUG
                    return true;
#else
                    return false;
#endif
                }

                return DEV_MODE_SW;
#pragma warning restore CS0162 // Unreachable code detected
            }
        }

        public static string DEV_APPSETTINGS => "appsettings.Development.json";
        public static string PROD_APPSETTINGS => "appsettings.json";

        public static string AppSettingsFileName
        {
            get
            {
                if (IsDevMode)
                    return DEV_APPSETTINGS;

                return PROD_APPSETTINGS;
            }
        }

        public static int MIN_PASSWORD_LENGTH => 8;
        public static int MAX_PASSWORD_LENGTH => 128;
        public static int MIN_USERNAME_LENGTH => 3;
    }
}
