namespace AdsPortal.WebApi.Exceptions.Handler
{
    using System;

    public struct ErrorModel
    {
        public string? PropertyName { get; }
        public string ErrorMessage { get; }

        public ErrorModel(string? propertyName, string errorMessage)
        {
            PropertyName = propertyName;
            ErrorMessage = errorMessage;
        }

        public override bool Equals(object? obj)
        {
            return obj is ErrorModel other &&
                   PropertyName == other.PropertyName &&
                   ErrorMessage == other.ErrorMessage;
        }

        public static bool operator ==(ErrorModel left, ErrorModel right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(ErrorModel left, ErrorModel right)
        {
            return !left.Equals(right);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(PropertyName, ErrorMessage);
        }

        public void Deconstruct(out string? propertyName, out string errorMessage)
        {
            propertyName = PropertyName;
            errorMessage = ErrorMessage;
        }

        public static implicit operator (string? PropertyName, string ErrorMessage)(ErrorModel value)
        {
            return (value.PropertyName, value.ErrorMessage);
        }

        public static implicit operator ErrorModel((string? PropertyName, string ErrorMessage) value)
        {
            return new ErrorModel(value.PropertyName, value.ErrorMessage);
        }
    }
}