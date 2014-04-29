using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace Bignum {
    public struct Integer {
        sbyte sign;
        Natural magnitude;

        public Integer(sbyte sign, Natural magnitude) {
            this.sign = sign;
            this.magnitude = magnitude;
        }

        public Integer(int value) {
            this.sign = value < 0 ? (sbyte)(-1) : (sbyte)1;
            this.magnitude = new Natural((ulong)Math.Abs((long)value));
        }

        public Integer(long value) {
            this.sign = value < 0 ? (sbyte)(-1) : (sbyte)1;
            if (value == Int64.MinValue)
                this.magnitude = new Natural((ulong)Int64.MaxValue) + 1;
            else
                this.magnitude = new Natural((ulong)Math.Abs(value));
        }

        public sbyte Sign {
            get { return sign; }
        }

        public Natural Magnitude {
            get { return magnitude; }
        }

        public static Integer operator -(Integer value) {
            return new Integer((sbyte) (-value.sign), value.magnitude);
        }
    }
}