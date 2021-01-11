namespace MagicOperations.Internal.Extensions
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    internal static class TypeExtensions
    {
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