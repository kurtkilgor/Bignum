using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace Bignum {
    public struct Natural {
        readonly uint tail;
        readonly uint[] head;

        public Natural(uint value) {
            this.head = null;
            this.tail = value;
        }       

        public Natural(ulong value) {
            this.tail = (uint)(value & 0xFFFFFFFF);
            value = value >> 32;
            uint chunk1 = (uint)(value & 0xFFFFFFFF);
            
            this.head = chunk1 == 0 ? null : new[] { chunk1 };            
        }

        #region Conversion Operators

        public static implicit operator Natural(uint value) {
            return new Natural(value);
        }

        public static implicit operator Natural(ulong value) {
            return new Natural(value);
        }

        public static explicit operator int(Natural bignum) {
            return (int)(uint)bignum;
        }

        public static explicit operator uint(Natural bignum) {
            if (bignum.head != null) 
                throw new OverflowException(); // Value too big.

            return bignum.tail;
        }

        public static explicit operator long(Natural bignum) {
            return (long)(ulong)bignum;
        }

        public static explicit operator ulong(Natural bignum) {
            if (bignum.head == null)
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

            if (a.head == null ^ b.head == null)
                return false;

            if (a.head != null) {
                if (a.head.Length != b.head.Length)
                    return false;

                for (int i = 0; i < a.head.Length; i++) {
                    if (a.head[i] != b.head[i])
                        return false;
                }
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
            value = value * 23 + (tail);
            if (head != null)
                foreach (var item in head) {
                    value = value * 23 + (item & 0x7FFFFFFF);
                }

            return (int) value;
        }

        #endregion
    }
}
