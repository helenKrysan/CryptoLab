using System;
using System.Collections.Generic;
using System.Text;

namespace EllipticCurve
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            // Generate new Keys
            PrivateKey privateKey = new PrivateKey();
            PublicKey publicKey = privateKey.publicKey();

            string message = "My test message";

            // Generate Signature
            Signature signature = Ecdsa.Sign(message, privateKey);

            // Verify if signature is valid
            Console.WriteLine(Ecdsa.Verify(message, signature, publicKey));
        }
    }
}
