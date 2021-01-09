namespace AdsPortal.Shared.Extensions.Extensions
{
    using System;

    //https://stackoverflow.com/questions/527486/c-enum-isdefined-on-combined-flags
    public static class EnumExtensions
    {
        //public static bool IsDefined(this Enum value)
        //{
        //    return !ulong.TryParse(value.ToString(), out _);
        //}

        public static bool IsDefined(this Enum value)
        {
            if (value == null)
            {
                return false;
            }

            //TODO: fix
            Array items = Enum.GetValues(value.GetType());
            foreach (Enum? item in items)
            {
                if (value.HasFlag(item!))
                {
                    return true;
                }
            }

            return false;
        }

        public static bool IsSingleFlagSet(this Enum value)
        {
            ulong x = (ulong)(object)value;

            return (x & (x - 1)) != 0;
        }
    }
}
