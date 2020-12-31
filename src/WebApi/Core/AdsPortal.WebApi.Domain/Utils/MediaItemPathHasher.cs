namespace AdsPortal.WebApi.Domain.Utils
{
    using System;
    using System.Data.HashFunction;
    using System.Data.HashFunction.MurmurHash;
    using AdsPortal.WebApi.Domain.Entities;

    public static class MediaItemPathHasher
    {
        private static readonly IMurmurHash2 _hasher = MurmurHash2Factory.Instance.Create(new MurmurHash2Config
        {
            HashSizeInBits = 64,
            Seed = 4978123U,
        });

        public static long CalculatePathHashCode(string path)
        {
            IHashValue? hashValue = _hasher.ComputeHash(path);

            return BitConverter.ToInt64(hashValue.Hash);
        }

        public static long CalculatePathHashCode(string virtualDirectory, string fileName)
        {
            string path = string.Concat(virtualDirectory, "/", fileName);
            IHashValue? hashValue = _hasher.ComputeHash(path);

            return BitConverter.ToInt64(hashValue.Hash);
        }

        public static long CalculatePathHashCode(this MediaItem mediaItem)
        {
            string path = string.Concat(mediaItem.VirtualDirectory, "/", mediaItem.FileName);
            IHashValue? hashValue = _hasher.ComputeHash(path);

            return BitConverter.ToInt64(hashValue.Hash);
        }
    }
}
