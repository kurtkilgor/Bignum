using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BignumTest {
    using Bignum;

    [TestClass]
    public class BasicTest {
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
        public void CreateFromInt() {
            var value = GetRandomInt();

            Assert.AreEqual(value, (int)new Bignum(value));
        }

        [TestMethod]
        public void CreateFromUint() {
            var value = GetRandomUint();

            Assert.AreEqual(value, (uint)new Bignum(value));
        }

        [TestMethod]
        public void CreateFromLong() {
            var value = GetRandomLong();

            Assert.AreEqual(value, (long)new Bignum(value));
        }

        [TestMethod]
        public void CreateFromUlong() {
            var value = GetRandomUlong();

            Assert.AreEqual(value, (ulong)new Bignum(value));
        }

        [TestMethod]
        [ExpectedException(typeof(OverflowException))]
        public void CastToIntOverflow() {
            var value = ((uint)Int32.MaxValue) + 1;
            var bignum = new Bignum(value);

            Assert.AreEqual(value, (uint)bignum);
            var intValue = (int)bignum; // Should overflow
        }

        [TestMethod]
        [ExpectedException(typeof(OverflowException))]
        public void CastNegativeToUintOverflow() {
            var value = -1;
            var bignum = new Bignum(value);

            Assert.AreEqual(value, (int)bignum);
            var uintValue = (uint)bignum; // Should overflow
        }

        [TestMethod]
        [ExpectedException(typeof(OverflowException))]
        public void CastNegativeToUlongOverflow() {
            var value = -1;
            var bignum = new Bignum(value);

            Assert.AreEqual(value, (int)bignum);
            var uintValue = (ulong)bignum; // Should overflow
        }

        [TestMethod]
        [ExpectedException(typeof(OverflowException))]
        public void CastUlongToUintOverflow() {
            var value = UInt64.MaxValue;
            var bignum = new Bignum(value);

            Assert.AreEqual(value, (ulong)bignum);
            var uintValue = (uint)bignum; // Should overflow
        }
    }
}
