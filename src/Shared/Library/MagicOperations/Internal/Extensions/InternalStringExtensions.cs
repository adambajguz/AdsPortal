namespace MagicOperations.Internal.Extensions
{
    using System;
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

        public static string TrimStart(this string source, string value, StringComparison comparisonType = StringComparison.CurrentCulture)
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (value.Length == 0)
            {
                return source;
            }
            else if (value.Length == 1)
            {
                return source.TrimStart(value[0]);
            }

            int valueLength = value.Length;
            int startIndex = 0;
            while (source.IndexOf(value, startIndex, comparisonType) == startIndex)
            {
                startIndex += valueLength;
            }

            return source[startIndex..];
        }

        public static string TrimEnd(this string source, string value, StringComparison comparisonType = StringComparison.CurrentCulture)
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (value.Length == 0)
            {
                return source;
            }
            else if (value.Length == 1)
            {
                return source.TrimEnd(value[0]);
            }

            int sourceLength = source.Length;
            int valueLength = value.Length;
            int count = sourceLength;
            while (source.LastIndexOf(value, count, comparisonType) == count - valueLength)
            {
                count -= valueLength;
            }

            return source.Substring(0, count);
        }
    }
}