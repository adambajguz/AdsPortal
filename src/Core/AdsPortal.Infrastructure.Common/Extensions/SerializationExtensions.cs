namespace AdsPortal.Infrastructure.Common.Extensions
{
    using System.IO;
    using System.Runtime.Serialization.Formatters.Binary;
    using Ceras;

    public static class Serialization
    {
        public static byte[] ToByteArray(this object? obj)
        {
            if (obj == null)
                return System.Array.Empty<byte>();

            using (MemoryStream memoryStream = new MemoryStream())
            {
                BinaryFormatter binaryFormatter = new BinaryFormatter();
                binaryFormatter.Serialize(memoryStream, obj);

                return memoryStream.ToArray();
            }
        }
        public static T? FromByteArray<T>(this byte[] byteArray)
            where T : class
        {
            if (byteArray == null)
                return default;

            using (MemoryStream memoryStream = new MemoryStream(byteArray))
            {
                BinaryFormatter binaryFormatter = new BinaryFormatter();

                return binaryFormatter.Deserialize(memoryStream) as T;
            }
        }

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