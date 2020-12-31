namespace AdsPortal.WebApi.Domain.Abstractions.Audit
{
    using System;
    using Abstractions.Enums;

    [AuditIgnore]
    public interface IAuditLog
    {
        string TableName { get; set; }
        Guid Key { get; set; }
        AuditActions Action { get; set; }
        string? Values { get; set; }
    }
}
