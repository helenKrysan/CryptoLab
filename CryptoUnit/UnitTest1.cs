using Microsoft.VisualStudio.TestTools.UnitTesting;
using CryptoLab1;
namespace CryptoUnit
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void KeyExpansionTest()
        {
            byte[] ckey = { 0x2b, 0x7e, 0x15, 0x16, 0x28, 0xae, 0xd2, 0xa6, 0xab, 0xf7, 0x15, 0x88, 0x09, 0xcf, 0x4f, 0x3c };

            AESEncryption encryption = new AESEncryption();

            var res = encryption.KeyExpansion(ckey);

            // Assert
            Assert.AreEqual(0xd014f9a8, res[9][0], 0.001, "Wrong key");
            Assert.AreEqual(0x19fadc21, res[8][1], 0.001, "Wrong key");
            Assert.AreEqual(0xe13f0cc8, res[9][2], 0.001, "Wrong key");
            Assert.AreEqual(0xb6630ca6, res[9][3], 0.001, "Wrong key");
        }

        [TestMethod]
        public void MixColumnsTest()
        {
            byte[][] array = new byte[][]{
                new byte[] {0x49, 0x45, 0x7f, 0x77 } ,
                new byte[] {0xdb, 0x39, 0x02, 0xde },
                new byte[] {0x87, 0x53, 0xd2, 0x96 },
                new byte[] {0x3b, 0x89, 0xf1, 0x1a } };
            AESEncryption encryption = new AESEncryption();

            var res = encryption.MixColumns(array);

            // Assert
            Assert.AreEqual(0x4b, res[1][1], 0.001, "Wrong key");
        }

        [TestMethod]
        public void SubBytesTest()
        {

            byte[][] array = new byte[][]{
                new byte[] {0xaa, 0x61, 0x82, 0x68 } ,
                new byte[] {0x8f, 0xdd, 0xd2, 0x32 },
                new byte[] {0x5f, 0xe3, 0x4a, 0x46 },
                new byte[] {0x03, 0xef, 0xd2, 0x9a } };
            AESEncryption encryption = new AESEncryption();

            var res = encryption.SubWord(array);

            // Assert
            Assert.AreEqual(0xc1, res[1][1], 0.001, "Wrong key");
        }

        [TestMethod]
        public void ShiftRowsTest()
        {
            byte[][] array = new byte[][]{
                new byte[] {0x49, 0x45, 0x7f, 0x77 } ,
                new byte[] {0xde ,0xdb, 0x39, 0x02},
                new byte[] { 0xd2, 0x96, 0x87, 0x53 },
                new byte[] { 0x89, 0xf1, 0x1a, 0x3b } };
            AESEncryption encryption = new AESEncryption();

            var res = encryption.ShiftRows(array);

            // Assert
            Assert.AreEqual(0x1a, res[3][3], 0.001, "Wrong key");
        }

        [TestMethod]
        public void RoundKeyTest()
        {
            byte[][] array = new byte[][]{
                new byte[] {0x25, 0xbd, 0xb6, 0x4c } ,
                new byte[] {0xd1 ,0x11, 0x3a, 0x4c},
                new byte[] { 0xa9, 0xd1, 0x33, 0xc0 },
                new byte[] { 0xad, 0x68, 0x8e, 0xb0 } };
            AESEncryption encryption = new AESEncryption();

            uint[] keys = { 0xd4d1c6f8, 0x7c839d87, 0xcaf2b8bc, 0x11f915bc };
            var res = encryption.RoundKey(array,keys);

            // Assert
            Assert.AreEqual(0x8b, res[2][2], 0.001, "Wrong key");
        }
    }
}
