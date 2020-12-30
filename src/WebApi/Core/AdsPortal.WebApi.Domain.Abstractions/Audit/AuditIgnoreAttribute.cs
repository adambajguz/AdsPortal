namespace AdsPortal.Domain.Abstractions.Audit
{
    using System;

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface | AttributeTargets.Property | AttributeTargets.Struct,
                    AllowMultiple = false,
                    Inherited = true)]
    public sealed class AuditIgnoreAttribute : Attribute
    {

    }
}
