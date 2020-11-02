using System;
using System.IO;
using System.Text;

namespace CryptoLab1
{
    internal class Program
    {
        private static string _fullFilePath = "D:/all_data/sudoku.csv";

        private static void IncreaseN(byte[] N)
        {
            bool F = true;
            int pos = N.Length - 1;
            while (F)
            {
                if (pos < 0)
                {
                    pos = N.Length - 1;
                }
                if (N[pos] < 255)
                {
                    N[pos] += 1;
                    F = false;
                }
                else
                {
                    N[pos] = 0;
                    pos--;
                }

            }
        }

        public static byte[] powKupyna()
        {
            Kupyna kupyna = new Kupyna();
            string datastr = "Hello";
            string hashStr = "";
            var w = Encoding.Default.GetBytes(datastr);
            byte[] x = new byte[64];
            for (int i = 0; i < w.Length; i++)
            {
                if (i > 64) break;
                x[i] = w[i];

            }
            hashStr = "";
            var hash0 = new byte[64];
            bool f = true;
            while (f)
            {
                IncreaseN(x);
                hash0 = kupyna.EncryptKupyna(x);
                var sum = 0;
                for (int j = 0; j < 5; j++)
                {
                    sum += hash0[j];
                }
                if (sum == 0)
                {
                    f = false;
                    for (int i = 0; i < 32; i++)
                    {
                        hashStr += string.Format("{0:X2}", hash0[i]);
                    }
                }

            }
            return hash0;

        }
        private static void Main(string[] args)
        {
            SHA256 sHA256 = new SHA256();
            string datastr = "Hello";
            string hashStr = "";
            var hash = sHA256.SHA256Encrypt(Encoding.Default.GetBytes(datastr));
            for (int i = 0; i < 32; i++)
            {
                hashStr += string.Format("{0:X2}", hash[i]);
            }

            Console.WriteLine(hashStr);
            var w = Encoding.Default.GetBytes(datastr);
            byte[] x = new byte[64];
            for(int i = 0; i< w.Length; i++)
            {
                if (i > 64) break;
                x[i] = w[i];

            }
            hashStr = "";
            bool f = true;
            /*while (f)
            {
                IncreaseN(x);
                var hash0 = sHA256.SHA256Encrypt(x);
                var sum = 0;
                for(int j = 0; j<5; j++)
                {
                    sum += hash0[j];
                }
                if(sum == 0)
                {
                    f = false;
                    for (int i = 0; i < 32; i++)
                    {
                        hashStr += string.Format("{0:X2}", hash0[i]);
                    }
                }

            }*/

            Kupyna kupyna = new Kupyna();
            datastr = "Hello";
            hashStr = "";
            hash = kupyna.EncryptKupyna(Encoding.Default.GetBytes(datastr));
            for (int i = 0; i < 32; i++)
            {
                hashStr += string.Format("{0:X2}", hash[i]);
            }

            Console.WriteLine(hashStr);
            
        }
    }
}