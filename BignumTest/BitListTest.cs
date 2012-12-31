using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BignumTest {
    using Bignum;

    [TestClass]
    public class BitListTest {
        [TestMethod]
        public void TestBitCount() {
            var value = BasicTest.GetRandomLong();
            var binaryValue = Convert.ToString(Math.Abs(value), 2);
            while(binaryValue[0] == '0')
                binaryValue = binaryValue.Substring(1);

            Assert.AreEqual(binaryValue.Length, (new Bignum(value)).BitList.Count);
        }

        [TestMethod]
        public void TestBitListIndex() {
            var value = BasicTest.GetRandomLong();
            var binaryValue = Convert.ToString(Math.Abs(value), 2);
            while (binaryValue[0] == '0')
                binaryValue = binaryValue.Substring(1);

            var randomIndex = (new Random()).Next(0, binaryValue.Length);

            Assert.AreEqual(binaryValue[randomIndex], (new Bignum(value)).BitList[randomIndex] ? '1' : '0');
        }

        [TestMethod]
        public void TestBitListEnumerator() {
            var value = BasicTest.GetRandomLong();
            var binaryValue = Convert.ToString(Math.Abs(value), 2);
            while (binaryValue[0] == '0')
                binaryValue = binaryValue.Substring(1);

            var bitList = (new Bignum(value)).BitList;

            var bitString = String.Empty;
            foreach (var bit in bitList) {
                bitString += bit ? '1' : '0';
            }

            Assert.AreEqual(binaryValue, bitString);
        }

    }
}
