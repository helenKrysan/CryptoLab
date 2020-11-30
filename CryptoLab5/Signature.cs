using System.Numerics;
using System;
using System.Collections.Generic;

namespace EllipticCurve {

    public class Signature {

        public BigInteger r { get; }
        public BigInteger s { get; }

        public Signature(BigInteger r, BigInteger s) {
            this.r = r;
            this.s = s;
        }

    }

}
