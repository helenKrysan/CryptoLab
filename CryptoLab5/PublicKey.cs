using System.Collections.Generic;
using System;


namespace EllipticCurve {

    public class PublicKey {

        public Point Point { get; }

        public CurveFp Curve { get; private set; }

        public PublicKey(Point point, CurveFp curve) {
            this.Point = point;
            this.Curve = curve;
        }

    }

}
