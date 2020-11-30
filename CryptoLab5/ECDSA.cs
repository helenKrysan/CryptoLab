using System.Security.Cryptography;
using System.Numerics;
using System.Text;

namespace EllipticCurve {

    public static class Ecdsa {

        public static Signature Sign(string message, PrivateKey privateKey) {
            string hashMessage = sha256(message);
            BigInteger numberMessage = Utils.BinaryAscii.numberFromHex(hashMessage);
            CurveFp curve = privateKey.curve;
            BigInteger k = Utils.Integer.RandomBetween(BigInteger.One, curve.N - 1);
            Point randSignPoint = EcdsaMath.Double(curve.G, k, curve.N, curve.A, curve.P);//point generation P
            BigInteger r = Utils.Integer.Modulo(randSignPoint.x, curve.N);
            BigInteger s = Utils.Integer.Modulo((numberMessage + r * privateKey.secret) * (EcdsaMath.inv(k, curve.N)), curve.N);

            return new Signature(r, s);
        }

        public static bool Verify(string message, Signature signature, PublicKey publicKey) {
            string hashMessage = sha256(message);
            BigInteger numberMessage = Utils.BinaryAscii.numberFromHex(hashMessage);
            CurveFp curve = publicKey.Curve;
            BigInteger sigR = signature.r;
            BigInteger sigS = signature.s;
            BigInteger inv = EcdsaMath.inv(sigS, curve.N);

            Point u1 = EcdsaMath.Double( curve.G, Utils.Integer.Modulo((numberMessage * inv), curve.N), curve.N, curve.A, curve.P );
            Point u2 = EcdsaMath.Double( publicKey.Point, Utils.Integer.Modulo((sigR * inv), curve.N), curve.N, curve.A, curve.P );
            Point add = EcdsaMath.Add( u1, u2, curve.A, curve.P );

            return sigR == add.x;
        }

        private static string sha256(string message) {
            byte[] bytes;

            using (SHA256 sha256Hash = SHA256.Create()) {
                bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(message));
            }

            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < bytes.Length; i++) {
                builder.Append(bytes[i].ToString("x2"));
            }

            return builder.ToString();
        }

    }

}
