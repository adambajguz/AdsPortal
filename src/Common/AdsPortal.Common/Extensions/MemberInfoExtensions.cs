namespace AdsPortal.Common.Extensions
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    public static class MemberInfoExtensions
    {
        // TODO: better cache
        // A static generic class ConcurrentDictionary for attributes caching
        private static class Attributes<T> where T : Attribute
        {
            private static readonly ConcurrentDictionary<MemberInfo, T[]> _cache = new ConcurrentDictionary<MemberInfo, T[]>();

            public static T[] Get(MemberInfo member)
            {
                return _cache.GetOrAdd(member,
                                       (key, arg) => GetAttributesUncached<T>(key),
                                       Array.Empty<T>());
            }
        }

        /// <summary>
        /// Checks or gets from cache, whether member has an attribute.
        /// </summary>
        public static bool HasAttribute<T>(this MemberInfo member)
            where T : Attribute
        {
            return Attributes<T>.Get(member).Any();
        }

        /// <summary>
        /// Whether member has an attribute.
        /// </summary>
        public static bool HasAttributeUncached<T>(this MemberInfo member)
            where T : Attribute
        {
            Type attributeType = typeof(T);

            // determine whether to inherit based on the AttributeUsage
            // you could add a bool parameter if you like but I think it defeats the purpose of the usage
            AttributeUsageAttribute? usage = attributeType.GetCustomAttributes(typeof(AttributeUsageAttribute), true)
                                                          .Cast<AttributeUsageAttribute>()
                                                          .FirstOrDefault();
            bool inherit = usage != null && usage.Inherited;

            if (inherit)
                return GetAttributesRecurse(member, attributeType).Any();

            return member.GetCustomAttributes(attributeType, false).Any();
        }

        /// <summary>
        /// Checks or gets from cache all member attributes, taking interfaces into account.
        /// Its recommended that attribute equivalence needs to be overridden because the default is not great.
        /// </summary>
        public static T[] GetAttributes<T>(this MemberInfo member)
            where T : Attribute
        {
            return Attributes<T>.Get(member);
        }

        /// <summary>
        /// Gets all member attributes, taking interfaces into account.
        /// Its recommended that attribute equivalence needs to be overridden because the default is not great.
        /// </summary>
        public static T[] GetAttributesUncached<T>(this MemberInfo member)
            where T : Attribute
        {
            Type attributeType = typeof(T);

            // Determine whether to inherit based on the AttributeUsage
            // You could add a bool parameter if you like but I think it defeats the purpose of the usage
            AttributeUsageAttribute? usage = attributeType.GetCustomAttributes(typeof(AttributeUsageAttribute), true)
                                                          .Cast<AttributeUsageAttribute>()
                                                          .FirstOrDefault();
            bool inherit = usage != null && usage.Inherited;

            return (
                inherit
                    ? GetAttributesRecurse(member, attributeType).Cast<T>()
                    : member.GetCustomAttributes(attributeType, false).Cast<T>()
                )
                .Distinct() // Interfaces mean duplicates are a thing
                .ToArray(); // Note: attribute equivalence needs to be overridden. The default is not great.
        }

        /// <summary>
        /// Gets all member attributes, taking interfaces into account.
        /// Its recommended that attribute equivalence needs to be overridden because the default is not great.
        /// </summary>
        public static Attribute[] GetAttributesUncached(this MemberInfo member, Type attributeType)
        {
            // determine whether to inherit based on the AttributeUsage
            // you could add a bool parameter if you like but I think it defeats the purpose of the usage
            AttributeUsageAttribute? usage = attributeType.GetCustomAttributes(typeof(AttributeUsageAttribute), true)
                .Cast<AttributeUsageAttribute>()
                .FirstOrDefault();
            bool inherit = usage?.Inherited ?? false;

            return (
                inherit
                    ? GetAttributesRecurse(member, attributeType)
                    : member.GetCustomAttributes(attributeType, false).Cast<Attribute>()
                )
                .Distinct() // interfaces mean duplicates are a thing
                .ToArray(); // note: attribute equivalence needs to be overridden. The default is not great.
        }

        private static IEnumerable<Attribute> GetAttributesRecurse(MemberInfo member, Type attributeType)
        {
            // must use Attribute.GetCustomAttribute rather than MemberInfo.GetCustomAttribute as the latter
            // won't retrieve inherited attributes from base *classes*
            foreach (Attribute attribute in Attribute.GetCustomAttributes(member, attributeType, true))
                yield return attribute;

            // The most reliable target in the interface map is the property get method.
            // If you have set-only properties, you'll need to handle that case. I generally just ignore that
            // case because it doesn't make sense to me.
            PropertyInfo? property;
            MemberInfo? target = (property = member as PropertyInfo) != null ? property.GetGetMethod() : member;

            foreach (Type @interface in member.DeclaringType!.GetInterfaces())
            {
                // The interface map is two aligned arrays; TargetMethods and InterfaceMethods.
                InterfaceMapping map = member.DeclaringType.GetInterfaceMap(@interface);

                int memberIndex = Array.IndexOf(map.TargetMethods, target); // see target above
                if (memberIndex < 0)
                    continue;

                // To recurse, we still need to hit the property on the parent interface.
                // Why don't we just use the get method from the start? Because GetCustomAttributes won't work.
                MemberInfo? interfaceMethod = property != null
                    // name of property get method is get_<property name>
                    // so name of parent property is substring(4) of that - this is reliable IME
                    ? @interface.GetProperty(map.InterfaceMethods[memberIndex].Name.Substring(4))
                    : (MemberInfo)map.InterfaceMethods[memberIndex];

                if (interfaceMethod != null)
                {
                    foreach (Attribute attribute in interfaceMethod.GetAttributesUncached(attributeType))
                        yield return attribute;
                }
            }
        }
    }
}
