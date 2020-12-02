namespace AdsPortal.ManagementUI.Helpers
{
    using System;
    using AdsPortal.Common;
    using Serilog;

    /*
     * When:
     * <PropertyGroup>
     *     <EnableDefaultContentItems>false</EnableDefaultContentItems>
     * </PropertyGroup>
     * and
     * <ItemGroup>
     *     <None Include="wwwroot\**" CopyToOutputDirectory="Always" />
     * </ItemGroup>
     * are added to .csproj the content of wwwroot is not dupliaced in wwwroot/_content
     *
     * But with <EnableDefaultContentItems>false</EnableDefaultContentItems> Blazor pages do not work.
     * Instead of this use:
     *
     *  <PropertyGroup>
     *      <StaticWebAssetBasePath Condition="$(StaticWebAssetBasePath) == ''">/</StaticWebAssetBasePath>
     *  </PropertyGroup>
     */
    public static class MediaHelper
    {
        private const bool Use_ContentFolder = false;
        private const bool GloballyDisabledVersioning = false;

        //TODO read from config
        public const string AssetsFolderName = "assets/";
        public const string ImagesFolderName = "images/";
        public const string VideosFolderName = "videos/";
        public const string StylesheetsFolderName = "css/";
        public const string ScriptsFolderName = "js/";

        private static readonly string AssemblyFullName = typeof(MediaHelper).Assembly.GetName().Name ?? throw new NullReferenceException($"{nameof(AssemblyFullName)} is null in {nameof(MediaHelper)}");
        private static readonly string path = Use_ContentFolder ? $"/_content/{AssemblyFullName}/{AssetsFolderName}" : $"/{AssetsFolderName}";

        public static string AsAsset(string resource, AssetTypes type = AssetTypes.Unspecified, bool? overrideVersioning = null)
        {
            if (string.IsNullOrWhiteSpace(resource))
            {
                Log.Error("Invalid resource {Resource} supplied to {Operation}", resource, $"{nameof(MediaHelper)}.{nameof(AsAsset)}");

                return path;
            }

            (string folder, bool versioned) = ResolveAssetFolderFromType(type);

            string tmp = path + folder + resource.Trim().TrimStart('/', '\\');

            if (!GloballyDisabledVersioning)
            {
                if (versioned || (overrideVersioning is bool b && b))
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
                AssetTypes.Script => (ScriptsFolderName, true),
                _ => (string.Empty, false),
            };
        }
    }
}
