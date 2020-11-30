using System.Globalization;
using System.Numerics;
using System.Text;
using System;


namespace EllipticCurve.Utils {

    public static class BinaryAscii {

        public static BigInteger numberFromHex(string hex) {
            if (((hex.Length % 2) == 1) || hex[0] != '0') {
                hex = "0" + hex; 
            }
            return BigInteger.Parse(hex, NumberStyles.HexNumber);
        }

    }

}