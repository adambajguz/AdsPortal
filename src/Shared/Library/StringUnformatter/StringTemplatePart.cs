namespace StringUnformatter
{
    using System;

    public readonly struct StringTemplatePart : IEquatable<StringTemplatePart>
    {
        public string Value { get; }
        public bool IsParameter { get; }

        public StringTemplatePart(string value, bool isParameter = false)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException($"'{nameof(value)}' cannot be null or whitespace", nameof(value));
            }

            Value = value;
            IsParameter = isParameter;
        }

        public override bool Equals(object? obj)
        {
            return obj is StringTemplatePart part && Equals(part);
        }

        public bool Equals(StringTemplatePart other)
        {
            return Value == other.Value &&
                   IsParameter == other.IsParameter;
        }

        public static bool operator ==(StringTemplatePart left, StringTemplatePart right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(StringTemplatePart left, StringTemplatePart right)
        {
            return !(left == right);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Value, IsParameter);
        }

        public override string? ToString()
        {
            return IsParameter ? $"PARAM: \"{{{Value}}}\"" : $"VALUE: \"{Value}\"";
        }
    }
}
