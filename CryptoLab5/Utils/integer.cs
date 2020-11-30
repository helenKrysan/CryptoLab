using System;
using System.Numerics;


namespace EllipticCurve.Utils {

    public static class Integer {

        static Random random = new Random();

        public static BigInteger Modulo(BigInteger dividend, BigInteger divisor) {
            BigInteger remainder = BigInteger.Remainder(dividend, divisor);
            
            if (remainder < 0) {
                return remainder + divisor;
            }

            return remainder;
        }

        public static BigInteger RandomBetween(BigInteger minimum, BigInteger maximum) {
            if (maximum < minimum) {
                throw new ArgumentException("maximum must be greater than minimum");
            }

            BigInteger range = maximum - minimum;

            Tuple<int, BigInteger> response = CalculateParameters(range);
            int bytesNeeded = response.Item1;
            BigInteger mask = response.Item2;

            byte[] randomBytes = new byte[bytesNeeded];
            random.NextBytes(randomBytes);

            BigInteger randomValue = new BigInteger(randomBytes);

            randomValue &= mask;

            if (randomValue <= range) {
                return minimum + randomValue;
            }

            return RandomBetween(minimum, maximum);

        }

        private static Tuple<int, BigInteger> CalculateParameters(BigInteger range) {
            int bitsNeeded = 0;
            int bytesNeeded = 0;
            BigInteger mask = new BigInteger(1);

            while (range > 0) {
                if (bitsNeeded % 8 == 0) {
                    bytesNeeded += 1;
                }

                bitsNeeded++;

                mask = (mask << 1) | 1;

                range >>= 1;
            }

            return Tuple.Create(bytesNeeded, mask);

        }

    }

}
