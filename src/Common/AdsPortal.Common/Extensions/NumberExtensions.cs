namespace AdsPortal.Common.Extensions
{
    public static class NumberExtensions
    {
        public static bool IsExactlyOneBitSet(this int i)
        {
            return i != 0 && (i & (i - 1)) == 0;
        }
    }
}
