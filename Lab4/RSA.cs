using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace CryptoLab1
{
    class RSA
    {
        public const int k = 2000;
        public int length;
        public BigInteger pk;
        public BigInteger sk;
        public BigInteger N;



        public RSA(int leng)
        {
            length = leng;
        }

        public BigInteger Encrypt(BigInteger m)
        {

            BigInteger p = 4;
            BigInteger q = 4;
            Random random = new Random();
            while(!Miller_Rabin.IsPrime(p,k))
            {
                byte[] bytes = new byte[length/8];
                random.NextBytes(bytes);
                bytes[length/8 -1] = 0;
                p = new BigInteger(bytes);
            }
            while (!Miller_Rabin.IsPrime(q, k))
            {
                byte[] bytes = new byte[length / 8];
                random.NextBytes(bytes);
                bytes[length / 8 - 1] = 0;
                q = new BigInteger(bytes);
            }
            var N = p * q;
            var fiN = countFi(p, q);
            var e = fiN;
            while(BigInteger.GreatestCommonDivisor(fiN,e) != 1)
            {
                byte[] bytes = fiN.ToByteArray();
                random.NextBytes(bytes);
                bytes[bytes.Length- 1] = 0;
                e = new BigInteger(bytes);                
            }
            var d = modInverse(e, fiN);
            sk = d;
            pk = e;
            this.N = N;
            var c = BigInteger.ModPow(m, pk, N);
            return c;
        }
        public BigInteger Decrypt(BigInteger c)
        {
            var m = BigInteger.ModPow(c, sk, N);
            return m;
        }
        BigInteger modInverse(BigInteger a, BigInteger n)
        {
            BigInteger i = n, v = 0, d = 1;
            while (a > 0)
            {
                BigInteger t = i / a, x = a;
                a = i % x;
                i = x;
                x = d;
                d = v - t * x;
                v = x;
            }
            v %= n;
            if (v < 0) v = (v + n) % n;
            return v;
        }
        public BigInteger RSA_OAEP(byte[] m)
        {
            int k1 = 32;
            int k0 = 32;
            var longm = new byte[length - k0];
            Array.Copy(m, longm, m.Length);
            for (int i = m.Length; i<length-k0-k1; i++ )
            {
                longm[i] = 0;
            }
            var bytes1 = new byte[256];
            Random r = new Random();
            r.NextBytes(bytes1);
            SHA256 sHA256 = new SHA256();
            var res = sHA256.SHA256Encrypt(bytes1);
            for(int i = res.Length-1; i>=0; i--)
            {
                longm[i] ^= res[i];
            }

            sHA256 = new SHA256();
            var res2 = sHA256.SHA256Encrypt(longm);
            for (int i = res2.Length-1; i >= 0; i--)
            {
                res2[i] ^= res[i];
            }
            var padded = new byte[length];
            Array.Copy(longm, padded, longm.Length);
            Array.Copy(res2, 0, padded, longm.Length, res2.Length);
            return new BigInteger(padded);
        }


        private BigInteger countFi(BigInteger p, BigInteger q)
        {
            return (p - 1) * (q - 1);
        }

        private BigInteger GCD(BigInteger a, BigInteger b)
        {
            while (a != 0 && b != 0)
            {
                if (a > b)
                    a %= b;
                else
                    b %= a;
            }
            return a | b;
        }

        public int GCDExtended(int a, int b,
                                  int x, int y)
        {
            if (a == 0)
            {
                x = 0;
                y = 1;
                return b;
            }
            int x1 = 1, y1 = 1;
            int gcd = GCDExtended(b % a, a, x1, y1);

            x = y1 - (b / a) * x1;
            y = x1;

            return gcd;
        }
    }
}
