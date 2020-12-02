namespace AdsPortal.Application.OperationsAbstractions
{
    using System;

    public interface IPatchProperty
    {
        bool Include { get; init; }
        public object? Value { get; }
        public Type HoldingType { get; }

        bool Equals(object? obj);
        int GetHashCode();
    }
}