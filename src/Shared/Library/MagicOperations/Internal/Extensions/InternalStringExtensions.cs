namespace MagicOperations.Internal.Extensions
{
    using System.Collections.Generic;

    internal static class InternalStringExtensions
    {
        public static string Quote(this string str)
        {
            return string.Concat("\"", str, "\"");
        }

        public static string JoinToString<T>(this IEnumerable<T> source, char separator)
        {
            return string.Join(separator, source);
        }
    }
}