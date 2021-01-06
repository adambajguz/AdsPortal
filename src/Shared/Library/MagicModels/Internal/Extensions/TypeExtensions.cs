namespace MagicModels.Internal.Extensions
{
    using System;
    using System.Collections.Generic;

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
                {
                    yield return currentBaseType;
                }

                currentBaseType = currentBaseType.BaseType;
            }
        }
    }
}