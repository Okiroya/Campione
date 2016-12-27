using System;

namespace Okiroya.Campione.Service.Cache
{
    /// <summary>
    /// Хеш-функция Дженкинса (One-at-a-Time)
    /// </summary>
    public static class OneAtaTimeHash
    {
        public static long GetHash(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return 0L;
            }
            else
            {
                long hash = 0L;
                for (int i = 0; i < input.Length; i++)
                {
                    hash += input[i];
                    hash += (hash << 10);
                    hash ^= (hash >> 6);
                }

                hash += (hash << 3);
                hash ^= (hash >> 11);
                hash += (hash << 15);

                return hash;
            }
        }
    }
}
