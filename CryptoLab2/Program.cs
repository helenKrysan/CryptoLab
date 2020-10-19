﻿using System;
using System.IO;

namespace CryptoLab1
{
	//tested on 1,5 gb
    internal class Program
    {
        private static string _fullFilePath = "pathtofile";

        private static void Main(string[] args)
        {
            Random _random = new Random();

            byte[] key = new byte[128];
            _random.NextBytes(key);

            //RC4
			//56,8 sec
            using (FileStream stream = new FileStream(_fullFilePath, FileMode.Open))
            {
                using (FileStream writeStream = new FileStream("D:/test_write.txt", FileMode.Create))
                {
                    var encryptRC4 = new RC4(key);
                    encryptRC4.Encode(stream, writeStream);
                }
            }

            /*using (FileStream stream = new FileStream("D:/test_write.txt", FileMode.Open))
            {
                using (FileStream writeStream = new FileStream("D:/test_decoded.csv", FileMode.Create))
                {
                    var encryptRC4 = new RC4(key);
                    encryptRC4.Encode(stream, writeStream);
                }
            }*/

			//use custom key to have the same result each time can be random
            byte[] key2 = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32 };
            byte[] nonce = { 3, 1, 4, 1, 5, 9, 2, 6 };

            //Salsa20
			//11,32min
            using (FileStream stream = new FileStream(_fullFilePath, FileMode.Open))
            {
                using (FileStream writeStream = new FileStream("D:/test_write_salsa.txt", FileMode.Create))
                {
                    var encryptSalsa = new Salsa20(key2, nonce);
                    while (true)
                    {
                        var block = new byte[8];
                        var count = stream.Read(block);
                        var res = encryptSalsa.Encrypt(block);
                        if (count < 8)
                        {
                            break;
                        }
                        for (int i = 0; i < 2; i++)
                        {
                            writeStream.Write(BitConverter.GetBytes(res[i]));
                        }
                    }
                }
            }

            /* using (FileStream stream = new FileStream("D:/test_write_salsa.txt", FileMode.Open))
             {
                 using (FileStream writeStream = new FileStream("D:/test_decoded_salsa.csv", FileMode.Create))
                 {
                     var encryptSalsa = new Salsa20(key2, nonce);
                     while (true)
                     {
                         var block = new byte[8];
                         var count = stream.Read(block);
                         var res = encryptSalsa.Encrypt(block);
                         if (count < 8)
                         {
                             break;
                         }
                         var t = BitConverter.GetBytes(res[0]);
                         writeStream.Write(BitConverter.GetBytes(res[0]));
                         writeStream.Write(BitConverter.GetBytes(res[1]));
                     }
                 }
             }*/
            //aes mode
			//ecv ~ 18 min
            var sma = new StreamModeAES();
            using (FileStream stream = new FileStream(_fullFilePath, FileMode.Open))
            {
                using (FileStream writeStream = new FileStream("D:/test_write_ecv.txt", FileMode.Create))
                {
                    sma.ModeECB(stream, writeStream);
                }
            }
        }
    }
}