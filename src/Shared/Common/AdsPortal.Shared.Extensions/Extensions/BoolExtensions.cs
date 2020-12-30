namespace AdsPortal.Shared.Extensions.Extensions
{
    public static class BoolExtensions
    {
        public static int ToInt(this bool b)
        {
            return b ? 1 : 0;
        }

        public static char ToChar(this bool b)
        {
            return b ? '1' : '0';
        }

        public static char ToCharT(this bool b)
        {
            return b ? 'T' : 'F';
        }
    }
}
