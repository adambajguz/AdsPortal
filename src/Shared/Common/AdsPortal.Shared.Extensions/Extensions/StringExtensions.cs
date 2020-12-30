namespace AdsPortal.Shared.Extensions.Extensions
{
    using System.Linq;

    public static class StringExtensions
    {
        public static string RemoveAllWhitespaces(this string input)
        {
            return new string(input.ToCharArray()
                .Where(c => !char.IsWhiteSpace(c))
                .ToArray());
        }

        public static string RemoveChar(this string input, char ch)
        {
            return new string(input.ToCharArray()
                .Where(c => c != ch)
                .ToArray());
        }

        public static string RemoveChar(this string input, params char[] ch)
        {
            return new string(input.ToCharArray()
                .Where(c => ch.Contains(c))
                .ToArray());
        }
    }
}
