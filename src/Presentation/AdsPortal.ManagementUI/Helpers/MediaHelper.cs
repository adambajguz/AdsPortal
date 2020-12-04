namespace AdsPortal.ManagementUI.Helpers
{
    using System;
    using AdsPortal.Common;
    using Serilog;

    public static class MediaHelper
    {
        private const bool Use_ContentFolder = true;
        private const bool GloballyDisabledVersioning = false;

        //TODO read from config
        public const string AssetsFolderName = "assets/";
        public const string ImagesFolderName = "images/";
        public const string VideosFolderName = "videos/";
        public const string StylesheetsFolderName = "css/";
        public const string FontsFolderName = "fonts/";
        public const string ScriptsFolderName = "js/";

        private static readonly string AssemblyFullName = typeof(MediaHelper).Assembly.GetName().Name ??
                                                              throw new NullReferenceException($"{nameof(AssemblyFullName)} is null in {nameof(MediaHelper)}");

        private static readonly string WwwRootPath = Use_ContentFolder ? $"/_content/{AssemblyFullName}/" : $"/";
        private static readonly string AssetsPath = $"{WwwRootPath}{AssetsFolderName}/";

        public static string AsAsset(string resource, AssetTypes type = AssetTypes.Unspecified, bool? overrideVersioning = null)
        {
            if (string.IsNullOrWhiteSpace(resource))
            {
                Log.Error("Invalid resource {Resource} supplied to {Operation}", resource, $"{nameof(MediaHelper)}.{nameof(AsAsset)}");

                return AssetsPath;
            }

            (string folder, bool versioned) = ResolveAssetFolderFromType(type);

            string tmp = AssetsPath + folder + resource.Trim().TrimStart('/', '\\');

            if (!GloballyDisabledVersioning)
            {
                if (versioned || (overrideVersioning is bool b && b))
                    return tmp + "?v=" + GlobalAppConfig.AppInfo.AppVersionTextPlain;
            }

            return tmp;
        }

        public static string AsRoot(string resource, bool versioned = false)
        {
            if (string.IsNullOrWhiteSpace(resource))
            {
                Log.Error("Invalid resource {Resource} supplied to {Operation}", resource, $"{nameof(MediaHelper)}.{nameof(AsRoot)}");

                return WwwRootPath;
            }

            string tmp = WwwRootPath + resource.Trim().TrimStart('/', '\\');

            if (!GloballyDisabledVersioning)
            {
                if (versioned)
                    return tmp + "?v=" + GlobalAppConfig.AppInfo.AppVersionTextPlain;
            }

            return tmp;
        }

        private static (string folder, bool versioned) ResolveAssetFolderFromType(AssetTypes type)
        {
            return type switch
            {
                AssetTypes.Image => (ImagesFolderName, false),
                AssetTypes.Video => (VideosFolderName, false),
                AssetTypes.Stylesheet => (StylesheetsFolderName, true),
                AssetTypes.Fonts => (FontsFolderName, false),
                AssetTypes.Script => (ScriptsFolderName, true),
                _ => (string.Empty, false),
            };
        }
    }
}
