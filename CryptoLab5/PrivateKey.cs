using System.Collections.Generic;
using System.Numerics;
using System;


namespace EllipticCurve
{

    public class PrivateKey
    {

        public CurveFp curve { get; private set; }
        public BigInteger secret { get; private set; }

        public PrivateKey()
        {
            this.curve = Curves.getCurveByName("secp256k1");
            this.secret = Utils.Integer.RandomBetween(1, this.curve.N - 1); //our key   
        }

        public PublicKey publicKey()
        {
            Point publicPoint = EcdsaMath.Double(curve.G, secret, curve.N, curve.A, curve.P);
            return new PublicKey(publicPoint, curve);
        }

    }
}
