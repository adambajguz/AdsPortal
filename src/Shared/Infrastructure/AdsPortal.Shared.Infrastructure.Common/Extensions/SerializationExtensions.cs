namespace AdsPortal.Shared.Infrastructure.Common.Extensions
{
    using Ceras;

    public static class Serialization
    {
        public static byte[] ToByteArrayCeras<T>(this T? obj)
            where T : class
        {
            if (obj == null)
                return System.Array.Empty<byte>();

            CerasSerializer ceras = new CerasSerializer();

            return ceras.Serialize(obj);
        }

        public static T? FromByteArrayCeras<T>(this byte[] byteArray)
            where T : class
        {
            if (byteArray == null)
                return default;

            CerasSerializer ceras = new CerasSerializer();

            return ceras.Deserialize<T>(byteArray);
        }

    }
}