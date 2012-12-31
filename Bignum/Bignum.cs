using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace Bignum {
    public struct Bignum {
        // The bignum is composed of an int for the least significant bits
        // and uints for the higher ones.

        // The int carries the sign for all numbers. For numbers larger than an
        // int, it does not use 2s complement but sign magnitude. This allows
        // easy conversion of ints and avoids using an extra byte for the sign.
        readonly int tail;
        readonly uint[] head;

        public Bignum(int value) {
            this.head = null;
            this.tail = value;
        }

        public Bignum(uint value) {
            uint top = (value & 0x80000000) == 0 ? 0 : (uint)1;
            int bottom = (int)(value & 0x7FFFFFFF);

            this.head = top > 0 ? new[] { top } : null;
            this.tail = bottom;
        }

        public Bignum(long value) {
            int bottom = (int)(Math.Abs(value) & 0x7FFFFFFF);
            if (value < 0)
                bottom = (int) (((uint) bottom) | 0x80000000);

            uint top = (uint)((Math.Abs(value) >> 31) & 0xFFFFFFFF);

            this.head = top > 0 ? new[] { top } : null;
            this.tail = bottom;
        }

        public Bignum(ulong value) {
            int bottom = (int)(value & 0x7FFFFFFF);
            value = value >> 31;
            uint chunk1 = (uint)(value & 0xFFFFFFFF);
            uint chunk2 = (value & 0x100000000) == 0 ? 0 : (uint)1;

            if (chunk1 == 0 && chunk2 == 0)
                this.head = null;
            else if (chunk2 == 0)
                this.head = new[] { chunk1 };
            else
                this.head = new[] { chunk2, chunk1 };
            this.tail = bottom;
        }

        public static explicit operator int(Bignum bignum) {
            if (bignum.head != null)
                throw new OverflowException(); // Value too big.

            return bignum.tail;
        }

        public static explicit operator uint(Bignum bignum) {
            if (bignum.tail < 0)
                throw new OverflowException(); // Can't cast negative to a uint.

            uint value = (uint)bignum.tail;
            if (bignum.head != null) {
                if (bignum.head.Length > 1 || bignum.head[0] > 1)
                    throw new OverflowException(); // Value too big.

                value += bignum.head[0] << 31;
            }

            return value;
        }

        public static explicit operator long(Bignum bignum) {
            int sign = (bignum.tail & 0x80000000) == 0 ? 1 : -1;

            long value = bignum.tail & 0x7FFFFFFF;

            if (bignum.head != null) {
                if (bignum.head.Length > 1)
                    throw new OverflowException(); // Value too big.
                else if (bignum.head.Length == 1)
                    value += ((long)bignum.head[0]) << 31;
            }

            return value * sign;
        }

        public static explicit operator ulong(Bignum bignum) {
            if (bignum.tail < 0)
                throw new OverflowException(); // Can't cast negative to a ulong.

            ulong value = (ulong)bignum.tail;
            if (bignum.head != null) {
                if (bignum.head.Length > 2)
                    throw new OverflowException(); // Value too big.
                else if (bignum.head.Length == 1)
                    value += ((ulong)bignum.head[0]) << 31;
                else if (bignum.head.Length == 2) {
                    if (bignum.head[0] > 1)
                        throw new OverflowException(); // Value too big.

                    value += ((ulong)bignum.head[0]) << 63;
                    value += ((ulong)bignum.head[1]) << 31;
                }
            }

            return value;
        }
    }
}
