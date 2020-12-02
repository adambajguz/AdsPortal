namespace AdsPortal.Analytics
{
    using System.Text.RegularExpressions;
    using Microsoft.AspNetCore.Http;

    public static class PathStringExtensions
    {
        private const int MAX_PATH_LENGTH = 256;

        private const string QUERY_REPLACEMENT_TOKEN = "${__Q}";
        private const string GUID_REPLACEMENT_TOKEN = "${__UUID}";
        private const string TRIMMED_PATH_TOKEN = "${__TRM}";

        private static readonly Regex regex = new Regex(@"[({]?[a-fA-F0-9]{8}[-]?([a-fA-F0-9]{4}[-]?){3}[a-fA-F0-9]{12}[})]?",
                                                        RegexOptions.IgnoreCase | RegexOptions.Compiled | RegexOptions.CultureInvariant);

        public static string Sanitize(this PathString path)
        {
            string url = path.Value;

            string[] splitUrl = url.Split(new char[] { '?', '#' });
            string cleanedUrl = splitUrl[0];

            if (splitUrl.Length > 1)
                cleanedUrl += QUERY_REPLACEMENT_TOKEN;

            cleanedUrl = regex.Replace(cleanedUrl, GUID_REPLACEMENT_TOKEN);

            bool needsTrimming = cleanedUrl.Length > MAX_PATH_LENGTH;
            if (needsTrimming)
            {
                cleanedUrl.Substring(0, MAX_PATH_LENGTH);
                cleanedUrl += TRIMMED_PATH_TOKEN;
            }

            return cleanedUrl;
        }
    }
}
