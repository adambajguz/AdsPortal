namespace MagicModels.Internal.Extensions
{
    using System;
    using System.Linq;
    using System.Reflection;

    internal static class PropertyInfoExtensions
    {
        public static bool IsInitOnly(this PropertyInfo propertyInfo)
        {
            MethodInfo? setMethod = propertyInfo?.SetMethod;

            if (setMethod is null)
                return false;

            var isExternalInitType = typeof(System.Runtime.CompilerServices.IsExternalInit);
            return setMethod.ReturnParameter.GetRequiredCustomModifiers().Contains(isExternalInitType);
        }
    }
}