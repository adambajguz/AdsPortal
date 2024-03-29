﻿namespace AdsPortal.Shared.Extensions.Extensions
{
    using System;
    using System.Linq;
    using System.Text;

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

        public static string? GetNullIfNullOrEmpty(this string? input)
        {
            return string.IsNullOrEmpty(input) ? null : input;
        }

        public static string? GetNullIfNullOrWhitespace(this string? input)
        {
            return string.IsNullOrWhiteSpace(input) ? null : input;
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

        public static byte[] AsByteArray(this string? str)
        {
            return str is string s ? Encoding.UTF8.GetBytes(s) : Array.Empty<byte>();
        }

        public static string? AsString(this byte[]? array)
        {
            return array is byte[] b ? Encoding.UTF8.GetString(b) : null;
        }
    }
}
