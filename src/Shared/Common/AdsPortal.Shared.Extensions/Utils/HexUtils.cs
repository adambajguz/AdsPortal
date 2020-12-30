namespace AdsPortal.Shared.Extensions.Utils
{
    using System;
    using System.Linq;
    using System.Runtime.InteropServices;

    //https://ndportmann.com/breaking-records-with-core-3-0/
    public static unsafe class HexUtils
    {
        private static readonly uint[] Lookup32 = Enumerable.Range(0, 256).Select(i =>
         {
             string s = i.ToString("X2");
             if (BitConverter.IsLittleEndian)
                 return s[0] + ((uint)s[1] << 16);

             return s[1] + ((uint)s[0] << 16);
         }).ToArray();

        private static readonly uint* _lookup32UnsafeP = (uint*)GCHandle.Alloc(Lookup32, GCHandleType.Pinned).AddrOfPinnedObject();

        public static string ToHexString(byte[] bytes)
        {
            uint* lookupP = _lookup32UnsafeP;
            string? result = new string((char)0, bytes.Length * 2);

            fixed (byte* bytesP = bytes)
            fixed (char* resultP = result)
            {
                uint* resultP2 = (uint*)resultP;
                for (int i = 0; i < bytes.Length; i++)
                    resultP2[i] = lookupP[bytesP[i]];
            }

            return result;
        }
    }
}
