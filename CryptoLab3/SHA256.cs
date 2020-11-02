using System;
using System.Collections.Generic;
using System.Text;

namespace CryptoLab1
{
    class SHA256
    {
        public byte[] _data = new byte[64];
        public uint datalen;
        public uint[] bitlen = new uint[2];
        public uint[] state = new uint[8];

        private uint[] k = {
    0x428a2f98,0x71374491,0xb5c0fbcf,0xe9b5dba5,0x3956c25b,0x59f111f1,0x923f82a4,0xab1c5ed5,
    0xd807aa98,0x12835b01,0x243185be,0x550c7dc3,0x72be5d74,0x80deb1fe,0x9bdc06a7,0xc19bf174,
    0xe49b69c1,0xefbe4786,0x0fc19dc6,0x240ca1cc,0x2de92c6f,0x4a7484aa,0x5cb0a9dc,0x76f988da,
    0x983e5152,0xa831c66d,0xb00327c8,0xbf597fc7,0xc6e00bf3,0xd5a79147,0x06ca6351,0x14292967,
    0x27b70a85,0x2e1b2138,0x4d2c6dfc,0x53380d13,0x650a7354,0x766a0abb,0x81c2c92e,0x92722c85,
    0xa2bfe8a1,0xa81a664b,0xc24b8b70,0xc76c51a3,0xd192e819,0xd6990624,0xf40e3585,0x106aa070,
    0x19a4c116,0x1e376c08,0x2748774c,0x34b0bcb5,0x391c0cb3,0x4ed8aa4a,0x5b9cca4f,0x682e6ff3,
    0x748f82ee,0x78a5636f,0x84c87814,0x8cc70208,0x90befffa,0xa4506ceb,0xbef9a3f7,0xc67178f2
};

        public SHA256()
        {
            datalen = 0;
            bitlen[0] = 0;
            bitlen[1] = 0;
            state[0] = 0x6a09e667;
            state[1] = 0xbb67ae85;
            state[2] = 0x3c6ef372;
            state[3] = 0xa54ff53a;
            state[4] = 0x510e527f;
            state[5] = 0x9b05688c;
            state[6] = 0x1f83d9ab;
            state[7] = 0x5be0cd19;
        }

        private uint RotateByNLeft(uint x, byte n)
        {
            return ((x << n) | (x >> (32 - n)));
        }

        private uint RotateByNRight(uint x, byte n)
        {
            return (((x) >> (n)) | ((x) << (32 - (n))));
        }

        private uint G(uint x, uint y, uint z)
        {
            return (((x) & (y)) ^ (~(x) & (z)));
        }

        private uint F(uint x, uint y, uint z)
        {
            return (((x) & (y)) ^ ((x) & (z)) ^ ((y) & (z)));
        }

        private uint BigSigma0(uint x)
        {
            return (RotateByNRight(x, 2) ^ RotateByNRight(x, 13) ^ RotateByNRight(x, 22));
        }

        private uint BigSigma1(uint x)
        {
            return (RotateByNRight(x, 6) ^ RotateByNRight(x, 11) ^ RotateByNRight(x, 25));
        }

        private uint SmallSigma0(uint x)
        {
            return (RotateByNRight(x, 7) ^ RotateByNRight(x, 18) ^ ((x) >> 3));
        }

        private uint SmallSigma1(uint x)
        {
            return (RotateByNRight(x, 17) ^ RotateByNRight(x, 19) ^ ((x) >> 10));
        }

        public void SHA256Transform(byte[] data)
        {
            uint a, b, c, d, e, f, g, h, i, j, t1, t2;
            uint[] m = new uint[64];

            for (i = 0, j = 0; i < 16; ++i, j += 4)
            {
                m[i] = (uint)((data[j] << 24) | (data[j + 1] << 16) | (data[j + 2] << 8) | (data[j + 3]));
            }

            for (; i < 64; ++i)
            {
                m[i] = SmallSigma1(m[i - 2]) + m[i - 7] + SmallSigma0(m[i - 15]) + m[i - 16];
            }

            a = state[0];
            b = state[1];
            c = state[2];
            d = state[3];
            e = state[4];
            f = state[5];
            g = state[6];
            h = state[7];

            for (i = 0; i < 64; ++i)
            {
                t1 = h + BigSigma1(e) + G(e, f, g) + k[i] + m[i];
                t2 = BigSigma0(a) + F(a, b, c);
                h = g;
                g = f;
                f = e;
                e = d + t1;
                d = c;
                c = b;
                b = a;
                a = t1 + t2;
            }

            state[0] += a;
            state[1] += b;
            state[2] += c;
            state[3] += d;
            state[4] += e;
            state[5] += f;
            state[6] += g;
            state[7] += h;
        }

        public byte[] SHA256Encrypt(byte[] data)
        {
            byte[] hash = new byte[32];

            for (uint ii = 0; ii < data.Length; ++ii)
            {
                _data[datalen] = data[ii];
                datalen++;

                if (datalen == 64)
                {
                    SHA256Transform(_data);
                    addInt(ref bitlen[0], ref bitlen[1], 512);
                    datalen = 0;
                }
            }

            uint i = datalen;

            if (datalen < 56)
            {
                _data[i++] = 0x80;

                while (i < 56)
                {
                    _data[i++] = 0x00;
                }
            }
            else
            {
                _data[i++] = 0x80;

                while (i < 64)
                {
                    _data[i++] = 0x00;
                }

                SHA256Transform(_data);
            }

            addInt(ref bitlen[0], ref bitlen[1], datalen * 8);
            _data[63] = (byte)(bitlen[0]);
            _data[62] = (byte)(bitlen[0] >> 8);
            _data[61] = (byte)(bitlen[0] >> 16);
            _data[60] = (byte)(bitlen[0] >> 24);
            _data[59] = (byte)(bitlen[1]);
            _data[58] = (byte)(bitlen[1] >> 8);
            _data[57] = (byte)(bitlen[1] >> 16);
            _data[56] = (byte)(bitlen[1] >> 24);
            SHA256Transform(_data);

            for (i = 0; i < 4; ++i)
            {
                hash[i] = (byte)(((state[0]) >> (int)(24 - i * 8)) & 0x000000ff);
                hash[i + 4] = (byte)(((state[1]) >> (int)(24 - i * 8)) & 0x000000ff);
                hash[i + 8] = (byte)(((state[2]) >> (int)(24 - i * 8)) & 0x000000ff);
                hash[i + 12] = (byte)((state[3] >> (int)(24 - i * 8)) & 0x000000ff);
                hash[i + 16] = (byte)((state[4] >> (int)(24 - i * 8)) & 0x000000ff);
                hash[i + 20] = (byte)((state[5] >> (int)(24 - i * 8)) & 0x000000ff);
                hash[i + 24] = (byte)((state[6] >> (int)(24 - i * 8)) & 0x000000ff);
                hash[i + 28] = (byte)((state[7] >> (int)(24 - i * 8)) & 0x000000ff);
            }

            return hash;
        }


        static void addInt(ref uint a, ref uint b, uint c)
        {
            if (a > 0xffffffff - c) ++b; a += c;
        }

    }
}
