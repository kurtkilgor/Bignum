using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BignumTest {
    using Bignum;

    [TestClass]
    public class BasicNaturalTest {
        public static uint GetRandomUint() {
            var random = new Random();
            return (uint)(random.Next() + random.Next());
        }

        public static int GetRandomInt() {
            var random = new Random();
            var value = random.Next();
            var sign = random.Next(0, 2) == 0 ? -1 : 1;
            return value * sign;
        }

        public static ulong GetRandomUlong() {
            var random = new Random();
            return ((ulong)random.Next()) << 32 + random.Next();
        }

        public static long GetRandomLong() {
            var random = new Random();
            var value = ((long)random.Next()) << 32 + random.Next();
            var sign = random.Next(0, 2) == 0 ? -1 : 1;
            return value * sign;
        }

        [TestMethod]
        public void CreateFromUint() {
            var value = GetRandomUint();

            Assert.AreEqual(value, (uint)new Natural(value));
            Assert.AreEqual(UInt32.MinValue, (uint)new Natural(UInt32.MinValue));
            Assert.AreEqual(UInt32.MaxValue, (uint)new Natural(UInt32.MaxValue));

        }

        [TestMethod]
        public void CreateFromUlong() {
            var value = GetRandomUlong();

            Assert.AreEqual(value, (ulong)new Natural(value));
            Assert.AreEqual(UInt64.MinValue, (ulong)new Natural(UInt64.MinValue));
            Assert.AreEqual(UInt64.MaxValue, (ulong)new Natural(UInt64.MaxValue));

        }

        [TestMethod]
        [ExpectedException(typeof(OverflowException))]
        public void CastUlongToUintOverflow() {
            var value = UInt64.MaxValue;
            var bignum = new Natural(value);

            Assert.AreEqual(value, (ulong)bignum);
            var uintValue = (uint)bignum; // Should overflow
        }

        [TestMethod]
        public void BasicEquality() {
            var value = GetRandomUlong();

            var v1 = new Natural(value);
            var v2 = new Natural(value);

            Assert.IsTrue(v1.Equals(v2));
            Assert.IsTrue(v2.Equals(v1));
            Assert.IsTrue(v1 == v2);
            Assert.IsTrue(v1 == value);
            Assert.IsTrue(v2 == value);
        }

        [TestMethod]
        public void BasicAdditionMultiplicationTest() {
            var value = new Natural(ulong.MaxValue);

            Assert.AreEqual(value + value, value * 2);
            Assert.AreEqual(value + value + value, value * 3);
        }
    }
}
