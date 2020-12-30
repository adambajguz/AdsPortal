namespace AdsPortal.Common.Interfaces
{
    using System;

    public interface IMachineInfoService
    {
        DateTime Now { get; }
        int CurrentYear { get; }

        DateTime UtcNow { get; }

        bool IsWindows { get; }
    }
}
