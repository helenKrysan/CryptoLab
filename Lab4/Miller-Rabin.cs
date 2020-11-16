using System;
using System.Numerics;

namespace CryptoLab1
{
    internal static class Miller_Rabin
    {
        public static bool IsPrime(BigInteger n, int k)
        {
            if ((n < 2) || (n % 2 == 0))
            {
                return (n == 2);
            }

            BigInteger m = n - 1;
            while (m % 2 == 0) m >>= 1;

            Random r = new Random();
            for (int i = 0; i < k; i++)
            {
                int a = r.Next(int.MaxValue - 1) + 1;
                BigInteger b = m;
                BigInteger mod =BigInteger.ModPow(a,m,n);
                while (b != n - 1 && mod != 1 && mod != n - 1)
                {
                    mod = (mod * mod) % n;
                    b *= 2;
                }

                if (mod != n - 1 && b % 2 == 0)
                {
                    return false;
                }
            }
            return true;
        }
    }
}