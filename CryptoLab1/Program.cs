
using System;
using System.IO;

namespace CryptoLab1
{
    class Program
    {
        static string _fullFilePath = "D:/all_data/sudoku.csv";
        static void Main(string[] args)
        {
            byte[] data = { 0x32, 0x43, 0xf6, 0xa8, 0x88, 0x5a, 0x30, 0x8d, 0x31, 0x31, 0x98, 0xa2, 0xe0, 0x37, 0x07, 0x34 };
            byte[] ckey = { 0x2b, 0x7e, 0x15, 0x16, 0x28, 0xae, 0xd2, 0xa6, 0xab, 0xf7, 0x15, 0x88, 0x09, 0xcf, 0x4f, 0x3c };
            AESEncryption encryption = new AESEncryption();
            byte[] encrypted = encryption.Encrypt(data, ckey);
            var res = encryption.KeyExpansion(ckey);
            Console.WriteLine(System.Text.Encoding.UTF8.GetString(encrypted));

            //kalyna

            ulong[] _state = new ulong[4];
            ulong[][] _round_keys = new ulong[10 + 1][];
            for (int i = 0; i < 10 + 1; i++)
            {
                _round_keys[i] = new ulong[4];
            }
            ulong[] test = { 0x1716151413121110, 0x1f1e1d1c1b1a1918 };
            ulong[] key = { 0x0706050403020100, 0x0f0e0d0c0b0a0908 };
            Kalyna kalyna = new Kalyna();
            kalyna.KalynaKeyExpand(key, _state, _round_keys);
            var result = kalyna.KalynaEncipher(test, _state, _round_keys);
            Console.WriteLine(result[0]);
            //read big file and encrypt
            byte[] array = new byte[1000000000];

            using (FileStream fs = new FileStream(_fullFilePath, FileMode.Open, FileAccess.Read))
            {
                var count = fs.Read(array);

                long pos = 0;
                while (pos < count)
                {
                    byte[] small = new byte[128];
                    if (pos + 128 < count)
                    {
                        Array.Copy(array, pos, small, 0, 128);
                        encryption.Encrypt(small, ckey);
                    }
                    pos += 128;
                }
            }
        }
    }
}
