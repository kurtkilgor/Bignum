using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
//
namespace Bignum {
    public partial struct Natural {
        uint tail;
        uint[] head;

        static uint[] Empty = new uint[0];

        Natural(uint tail, uint[] head) {
            this.tail = tail;
            this.head = head;
        }

        public Natural(uint value) {
            this.head = Empty;
            this.tail = value;
        }       

        public Natural(ulong value) {
            this.tail = (uint)(value & 0xFFFFFFFF);
            value = value >> 32;
            uint chunk1 = (uint)(value & 0xFFFFFFFF);
            
            this.head = chunk1 == 0 ? Empty : new[] { chunk1 };            
        }

        #region Conversion Operators

        public static implicit operator Natural(uint value) {
            return new Natural(value);
        }

        public static implicit operator Natural(ulong value) {
            return new Natural(value);
        }

        public static explicit operator uint(Natural bignum) {
            if (bignum.head.Length > 0) 
                throw new OverflowException(); // Value too big.

            return bignum.tail;
        }

        public static explicit operator ulong(Natural bignum) {
            if (bignum.head.Length == 0)
                return bignum.tail;
            else if (bignum.head.Length == 1)
                return (((ulong)bignum.head[0]) << 32) + bignum.tail;
            else
                throw new OverflowException();
        }

        #endregion

        #region Equals and GetHashCode

        public static bool operator ==(Natural a, Natural b) {
            return a.Equals(b);
        }

        public static bool operator !=(Natural a, Natural b) {
            return !a.Equals(b);
        }

        public static bool Equals(Natural a, Natural b) {
            if (a.tail != b.tail)
                return false;

            if (a.head.Length != b.head.Length)
                return false;

            for (int i = 0; i < a.head.Length; i++) {
                if (a.head[i] != b.head[i])
                    return false;
            }
        
            return true;
        }

        public override bool Equals(object obj) {
            if (!(obj is Natural))
                return false;

            var bignum = (Natural)obj;
            return Equals(this, bignum);
        }

        public override int GetHashCode() {
            // Based on http://stackoverflow.com/a/263416
            uint value = 17;
            value = value * 23 + tail;

            foreach (var item in head) {
                value = value * 23 + item;
            }

            return (int) value;
        }

        #endregion

        #region Comparison Operators

        static bool CompareEqualLengthHeads(Natural a, Natural b, Func<uint, uint, bool> op) {
            for (var i = 0; i < a.head.Length; i++) {
                if (a.head[i] == b.head[i])
                    continue;

                return op(a.head[i], b.head[i]);
            }

            return op(a.tail, b.tail);
        }

        public static bool operator <(Natural a, Natural b) {
            int headALength = a.head.Length;
            int headBLength = b.head.Length;

            if (headALength == headBLength)
                return CompareEqualLengthHeads(a, b, (x, y) => x < y);
            else return headALength < headBLength;
        }

        public static bool operator >(Natural a, Natural b) {
            int headALength = a.head.Length;
            int headBLength = b.head.Length;

            if (headALength == headBLength)
                return CompareEqualLengthHeads(a, b, (x, y) => x > y);
            else return headALength > headBLength;
        }

        public static bool operator <=(Natural a, Natural b) {
            int headALength = a.head.Length;
            int headBLength = b.head.Length;

            if (headALength == headBLength)
                return CompareEqualLengthHeads(a, b, (x, y) => x <= y);
            else return headALength < headBLength;
        }

        public static bool operator >=(Natural a, Natural b) {
            int headALength = a.head.Length;
            int headBLength = b.head.Length;

            if (headALength == headBLength)
                return CompareEqualLengthHeads(a, b, (x, y) => x >= y);
            else return headALength > headBLength;
        }

        #endregion

        public override string ToString() {
            var s = new StringBuilder();
            var a = this;
            while(a > 0) {
                Natural quotient;
                Natural remainder;
                Divide(a, 10, out quotient, out remainder);
                var digit = (uint) remainder;
                s.Insert(0, digit.ToString());
                a = quotient;
            }

            return s.ToString();
        }
    }
}
