namespace MediatR.GenericOperations.Models
{
    using System;
    using MediatR.GenericOperations.Abstractions;

    public struct PatchProperty<T> : IPatchProperty
    {
        public bool Include { readonly get; init; }
        public T Value { readonly get; init; }
        object? IPatchProperty.Value => Value;
        Type IPatchProperty.HoldingType => typeof(T);

        public PatchProperty(T value)
        {
            Include = true;
            Value = value;
        }

        public PatchProperty(bool include, T value)
        {
            Include = include;
            Value = value;
        }

        public static bool operator ==(PatchProperty<T> left, PatchProperty<T> right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(PatchProperty<T> left, PatchProperty<T> right)
        {
            return !left.Equals(right);
        }

        public override readonly bool Equals(object? obj)
        {
            return obj is PatchProperty<T> other &&
                   Include == other.Include &&
                   (Value?.Equals(other.Value) ?? other.Value is null);
        }

        public override readonly int GetHashCode()
        {
            return HashCode.Combine(Include, Value);
        }

        public readonly void Deconstruct(out bool include, out T value)
        {
            include = Include;
            value = Value;
        }

        public static implicit operator (bool, T)(PatchProperty<T> value)
        {
            return (value.Include, value.Value);
        }

        public static implicit operator PatchProperty<T>((bool, T) value)
        {
            return new PatchProperty<T>(value.Item1, value.Item2);
        }

        public static explicit operator T(PatchProperty<T> patchProperty)
        {
            return patchProperty.Value;
        }

        public static explicit operator PatchProperty<T>(T value)
        {
            return new PatchProperty<T>(true, value);
        }
    }
}
