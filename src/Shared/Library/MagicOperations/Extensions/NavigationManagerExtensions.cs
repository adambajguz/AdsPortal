namespace MagicOperations.Extensions
{
    using System;
    using Microsoft.AspNetCore.Components;

    public static class NavigationManagerExtensions
    {
        public static string GetCurrentPageUri(this NavigationManager navigationManager)
        {
            Uri tmp = new Uri(navigationManager.Uri);

            return tmp.AbsolutePath;
        }

        public static string GetCurrentPageUriWithQuery(this NavigationManager navigationManager)
        {
            Uri tmp = new Uri(navigationManager.Uri);

            return tmp.PathAndQuery;
        }

        public static string BuildCurrentPageRelativeUri(this NavigationManager navigationManager, string uri)
        {
            return (navigationManager.GetCurrentPageUri() + "/" + uri).TrimStart('/');
        }
    }
}
