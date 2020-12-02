namespace AdsPortal.WebAPI.Attributes
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.AspNetCore.Authorization;

    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, Inherited = true, AllowMultiple = true)]
    public sealed class CustomAuthorizeAttribute : AuthorizeAttribute
    {
        public CustomAuthorizeAttribute(string policy) : base(policy)
        {

        }

        public CustomAuthorizeAttribute(params object[] roles)
        {
            //if (roles.Any(r => r.GetType().BaseType != typeof(Enum)))
            //    throw new ArgumentException("roles");

            IEnumerable<string?> values = roles.Select(r => r.ToString()); //Enum.GetName(r.GetType(), r)

            Roles = string.Join(", ", values);
        }
    }
}
