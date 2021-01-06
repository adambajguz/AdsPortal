﻿namespace MagicModels.Internal.Extensions
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    internal static class TypeExtensions
    {
        public static IEnumerable<Type> GetBaseTypes(this Type type)
        {
            Type? currentBaseType = type.BaseType;
            while (currentBaseType != null)
            {
                yield return currentBaseType;
                currentBaseType = currentBaseType.BaseType;
            }
        }

        public static IEnumerable<Type> GetBaseTypesOtherThanObject(this Type type)
        {
            Type? currentBaseType = type.BaseType;
            while (currentBaseType != null)
            {
                if (currentBaseType != typeof(object))
                    yield return currentBaseType;

                currentBaseType = currentBaseType.BaseType;
            }
        }

        public static Type? TryGetEnumerableArgumentUnderlyingType(this PropertyInfo? property)
        {
            return property != null && property.PropertyType != typeof(string)
                       ? property.PropertyType.TryGetEnumerableUnderlyingType()
                       : null;
        }

        public static Type? TryGetNullableUnderlyingType(this Type type)
        {
            return Nullable.GetUnderlyingType(type);
        }

        public static Type? TryGetEnumerableUnderlyingType(this Type type)
        {
            if (type.IsPrimitive)
                return null;

            if (type == typeof(IEnumerable))
                return typeof(object);

            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(IEnumerable<>))
                return type.GetGenericArguments().FirstOrDefault();

            return type.GetInterfaces()
                       .Select(TryGetEnumerableUnderlyingType)
                       .Where(t => t != null)
                       .OrderByDescending(t => t != typeof(object)) // prioritize more specific types
                       .FirstOrDefault();
        }

        public static MethodInfo? TryGetStaticParseMethod(this Type type, bool withFormatProvider = false)
        {
            Type[] argumentTypes = withFormatProvider
                ? new[] { typeof(string), typeof(IFormatProvider) }
                : new[] { typeof(string) };

            return type.GetMethod("Parse",
                                  BindingFlags.Public | BindingFlags.Static,
                                  null,
                                  argumentTypes,
                                  null);
        }

        public static Array ToNonGenericArray<T>(this IEnumerable<T> source, Type elementType)
        {
            ICollection sourceAsCollection = source as ICollection ?? source.ToArray();

            Array array = Array.CreateInstance(elementType, sourceAsCollection.Count);
            sourceAsCollection.CopyTo(array, 0);

            return array;
        }
    }
}