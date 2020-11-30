using System.Collections.Generic;
using System.Numerics;
using System;

namespace EllipticCurve
{
    public class CurveFp
    {
        public BigInteger A { get; private set; }
        public BigInteger B { get; private set; }
        public BigInteger P { get; private set; }
        public BigInteger N { get; private set; }
        public Point G { get; private set; }
        public string name { get; private set; }
        public int[] oid { get; private set; }
        public string nistName { get; private set; }


        public CurveFp(BigInteger A, BigInteger B, BigInteger P, BigInteger N, BigInteger Gx, BigInteger Gy, string name, int[] oid, string nistName = "") {
            this.A = A;
            this.B = B;
            this.P = P;
            this.N = N;
            G = new Point(Gx, Gy);
            this.name = name;
            this.nistName = nistName;
            this.oid = oid;
        }

    }

    public static class Curves {

        public static CurveFp getCurveByName(string name) {
            name = name.ToLower();

            if (name == "secp256k1") {
                return secp256k1;
            }
            throw new ArgumentException("unknown curve " + name);
        }

        public static CurveFp secp256k1 = new CurveFp(
            Utils.BinaryAscii.numberFromHex("1"),
            Utils.BinaryAscii.numberFromHex("5FF6108462A2DC8210AB403925E638A19C1455D21"),
            Utils.BinaryAscii.numberFromHex("fffffffffffffffffffffffffffffffffffffffffffffffffffffffefffffc2f"),
            Utils.BinaryAscii.numberFromHex("fffffffffffffffffffffffffffffffebaaedce6af48a03bbfd25e8cd0364141"),
            Utils.BinaryAscii.numberFromHex("79be667ef9dcbbac55a06295ce870b07029bfcdb2dce28d959f2815b16f81798"),
            Utils.BinaryAscii.numberFromHex("483ada7726a3c4655da4fbfc0e1108a8fd17b448a68554199c47d08ffb10d4b8"),
            "secp256k1",
            new int[] { 1, 3, 132, 0, 10 }
        );


    }

}
