namespace AdsPortal.Shared.Extensions.Extensions
{
    public static class NumberExtensions
    {
        public static bool IsExactlyOneBitSet(this int i)
        {
            return i != 0 && (i & (i - 1)) == 0;
        }
    }
}
