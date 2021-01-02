namespace MagicOperations.Internal
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Reflection;
    using MagicOperations.Extensions;
    using MagicOperations.Internal.Extensions;
    using MagicOperations.Schemas;

    internal static class ArgumentBinder
    {
        private static readonly IFormatProvider FormatProvider = CultureInfo.InvariantCulture;

        private static readonly IReadOnlyDictionary<Type, Func<string, object?>> PrimitiveConverters =
            new Dictionary<Type, Func<string, object?>>
            {
                [typeof(sbyte)] = v => sbyte.Parse(v, FormatProvider),
                [typeof(byte)] = v => byte.Parse(v, FormatProvider),
                [typeof(char)] = v => char.Parse(v),
                [typeof(short)] = v => short.Parse(v, FormatProvider),
                [typeof(ushort)] = v => ushort.Parse(v, FormatProvider),
                [typeof(int)] = v => int.Parse(v, FormatProvider),
                [typeof(uint)] = v => uint.Parse(v, FormatProvider),
                [typeof(long)] = v => long.Parse(v, FormatProvider),
                [typeof(ulong)] = v => ulong.Parse(v, FormatProvider),
                [typeof(Half)] = v => Half.Parse(v, FormatProvider),
                [typeof(float)] = v => float.Parse(v, FormatProvider),
                [typeof(double)] = v => double.Parse(v, FormatProvider),
                [typeof(decimal)] = v => decimal.Parse(v, FormatProvider),
                [typeof(DateTime)] = v => DateTime.Parse(v, FormatProvider),
                [typeof(DateTimeOffset)] = v => DateTimeOffset.Parse(v, FormatProvider),
                [typeof(TimeSpan)] = v => TimeSpan.Parse(v, FormatProvider),
            };

        private static object? ConvertScalar(string? value, Type targetType)
        {
            try
            {
                // No conversion necessary
                if (targetType == typeof(object) || targetType == typeof(string))
                    return value;

                // Bool conversion (special case)
                if (targetType == typeof(bool))
                    return string.IsNullOrWhiteSpace(value) || bool.Parse(value);

                // Primitive conversion
                Func<string, object?>? primitiveConverter = PrimitiveConverters.GetValueOrDefault(targetType);
                if (primitiveConverter is not null && !string.IsNullOrWhiteSpace(value))
                    return primitiveConverter(value);

                // Enum conversion conversion
                if (targetType.IsEnum && !string.IsNullOrWhiteSpace(value))
                    return Enum.Parse(targetType, value ?? string.Empty, true);

                // Nullable<T> conversion
                Type? nullableUnderlyingType = targetType.TryGetNullableUnderlyingType();
                if (nullableUnderlyingType is not null)
                    return !string.IsNullOrWhiteSpace(value)
                        ? ConvertScalar(value, nullableUnderlyingType)
                        : null;

                // String-constructible conversion
                ConstructorInfo? stringConstructor = targetType.GetConstructor(new[] { typeof(string) });
                if (stringConstructor is not null)
                    return stringConstructor.Invoke(new object?[] { value });

                // String-parseable (with format provider) conversion
                MethodInfo? parseMethodWithFormatProvider = targetType.TryGetStaticParseMethod(true);
                if (parseMethodWithFormatProvider is not null)
                    return parseMethodWithFormatProvider.Invoke(null, new object[] { value!, FormatProvider });

                // String-parsable (without format provider) conversion
                MethodInfo? parseMethod = targetType.TryGetStaticParseMethod();
                if (parseMethod is not null)
                    return parseMethod.Invoke(null, new object?[] { value });
            }
            catch (Exception ex)
            {
                throw new ArgumentBinderException($"Cannot convert value '{value}' to type '{targetType.FullName}'.", ex);
            }

            throw new ArgumentBinderException($"No conversion for value '{value}' to type '{targetType.FullName}'.");
        }

        private static object ConvertNonScalar(IReadOnlyList<string> values, Type targetEnumerableType, Type targetElementType)
        {
            Array array = values.Select(v => ConvertScalar(v, targetElementType))
                                .ToNonGenericArray(targetElementType);

            Type arrayType = array.GetType();

            // Assignable from an array
            if (targetEnumerableType.IsAssignableFrom(arrayType))
                return array;

            // Constructible from an array
            ConstructorInfo? arrayConstructor = targetEnumerableType.GetConstructor(new[] { arrayType });
            if (arrayConstructor is not null)
                return arrayConstructor.Invoke(new object[] { array });

            throw new ArgumentBinderException($"Cannot convert provided values to type '{ targetEnumerableType.Name }': { values.Select(v => v.Quote()).JoinToString(' ')}. " +
                                              $"Target type is not assignable from array and doesn't have a public constructor that takes an array.");
        }

        public static object? Convert(this OperationArgument argument)
        {
            PropertyInfo? property = argument.Schema.Property;
            string value = argument.Value;
            string[] values = value.Split(',');

            // Short-circuit built-in arguments
            if (property is null)
                return null;

            Type targetType = property.PropertyType;
            Type? enumerableUnderlyingType = property.TryGetEnumerableArgumentUnderlyingType();

            // Scalar
            if (enumerableUnderlyingType is null)
            {
                return values.Length <= 1
                    ? ConvertScalar(values.SingleOrDefault(), targetType)
                    : throw new ArgumentBinderException($"Parameter {property.Name} expects a single value, but provided with multiple: { values.Select(v => v.Quote()).JoinToString(' ') }");
            }
            // Non-scalar
            else
                return ConvertNonScalar(values, targetType, enumerableUnderlyingType);
        }
    }
}
