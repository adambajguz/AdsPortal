namespace MagicOperations.Internal.Extensions
{
    using System;
    using System.Linq;

    internal static class TypeExtensions
    {
        public static bool Implements(this Type type, Type interfaceType)
        {
            return type.GetInterfaces().Contains(interfaceType);
        }
    }
}