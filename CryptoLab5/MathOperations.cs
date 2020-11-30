using System.Numerics;


namespace EllipticCurve {

    public static class EcdsaMath {

        public static Point Double (Point p, BigInteger n, BigInteger N, BigInteger A, BigInteger P) {

            return fromJacobian(
                jacobianMultiply(
                    toJacobian(p),
                    n,
                    N,
                    A,
                    P
                ),
                P
            );
        }

        public static Point Add (Point p, Point q, BigInteger A, BigInteger P) {

            return fromJacobian(
                jacobianAdd(
                    toJacobian(p),
                    toJacobian(q),
                    A,
                    P
                ),
                P
            );
        }

        public static BigInteger inv (BigInteger x, BigInteger n) {
            if (x.IsZero) {
                return 0;
            }

            BigInteger lm = BigInteger.One;
            BigInteger hm = BigInteger.Zero;
            BigInteger low = Utils.Integer.Modulo(x, n);
            BigInteger high = n;
            BigInteger r, nm, newLow;

            while (low > 1) {
                r = high / low;

                nm = hm - (lm * r);
                newLow = high - (low * r);

                high = low;
                hm = lm;
                low = newLow;
                lm = nm;
            }

            return Utils.Integer.Modulo(lm, n);

        }

        private static Point toJacobian (Point p) {

            return new Point(p.x, p.y, 1);
        }

        private static Point fromJacobian (Point p, BigInteger P) {

            BigInteger z = inv(p.z, P);

            return new Point(
                Utils.Integer.Modulo(p.x * BigInteger.Pow(z, 2), P),
                Utils.Integer.Modulo(p.y * BigInteger.Pow(z, 3), P)
            );
        }

        private static Point jacobianDouble (Point p, BigInteger A, BigInteger P) {

            if (p.y.IsZero) {
                return new Point(
                    BigInteger.Zero,
                    BigInteger.Zero,
                    BigInteger.Zero
                );
            }

            BigInteger ysq = Utils.Integer.Modulo(
                BigInteger.Pow(p.y, 2),
                P
            );
            BigInteger S = Utils.Integer.Modulo(
                4 * p.x * ysq,
                P
            );
            BigInteger M = Utils.Integer.Modulo(
                3 * BigInteger.Pow(p.x, 2) + A * BigInteger.Pow(p.z, 4),
                P
            );

            BigInteger nx = Utils.Integer.Modulo(
                BigInteger.Pow(M, 2) - 2 * S,
                P
            );
            BigInteger ny = Utils.Integer.Modulo(
                M * (S - nx) - 8 * BigInteger.Pow(ysq, 2),
                P
            );
            BigInteger nz = Utils.Integer.Modulo(
                2 * p.y * p.z,
                P
            );

            return new Point(
                nx,
                ny,
                nz
            );
        }

        private static Point jacobianAdd (Point p, Point q, BigInteger A, BigInteger P) {

            if (p.y.IsZero) {
                return q;
            }
            if (q.y.IsZero) {
                return p;
            }

            BigInteger U1 = Utils.Integer.Modulo(
                p.x * BigInteger.Pow(q.z, 2),
                P
            );
            BigInteger U2 = Utils.Integer.Modulo(
                q.x * BigInteger.Pow(p.z, 2),
                P
            );
            BigInteger S1 = Utils.Integer.Modulo(
                p.y * BigInteger.Pow(q.z, 3),
                P
            );
            BigInteger S2 = Utils.Integer.Modulo(
                q.y * BigInteger.Pow(p.z, 3),
                P
            );

            if (U1 == U2) {
                if (S1 != S2) {
                    return new Point(BigInteger.Zero, BigInteger.Zero, BigInteger.One);
                }
                return jacobianDouble(p, A, P);
            }

            BigInteger H = U2 - U1;
            BigInteger R = S2 - S1;
            BigInteger H2 = Utils.Integer.Modulo(H * H, P);
            BigInteger H3 = Utils.Integer.Modulo(H * H2, P);
            BigInteger U1H2 = Utils.Integer.Modulo(U1 * H2, P);
            BigInteger nx = Utils.Integer.Modulo(
                BigInteger.Pow(R, 2) - H3 - 2 * U1H2,
                P
            );
            BigInteger ny = Utils.Integer.Modulo(
                R * (U1H2 - nx) - S1 * H3,
                P
            );
            BigInteger nz = Utils.Integer.Modulo(
                H * p.z * q.z,
                P
            );

            return new Point(
                nx,
                ny,
                nz
            );
        }

        private static Point jacobianMultiply (Point p, BigInteger n, BigInteger N, BigInteger A, BigInteger P) {

            if (p.y.IsZero | n.IsZero) {
                return new Point( BigInteger.Zero, BigInteger.Zero, BigInteger.One );
            }

            if (n.IsOne) {
                return p;
            }

            if (n < 0 | n >= N) {
                return jacobianMultiply( p, Utils.Integer.Modulo(n, N), N, A, P );
            }

            if (Utils.Integer.Modulo(n, 2).IsZero) {
                return jacobianDouble( jacobianMultiply( p, n / 2, N, A, P ), A, P );
            }

            // (n % 2) == 1:
            return jacobianAdd( jacobianDouble( jacobianMultiply( p, n / 2, N, A, P ), A, P ), p, A, P );

        }

    }

}
