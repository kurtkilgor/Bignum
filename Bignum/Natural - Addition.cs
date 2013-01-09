using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace Bignum {
    public partial struct Natural {
        public static Natural operator +(Natural a, Natural b) {
            var uintsA = new List<uint>(a.WalkUints());
            var uintsB = new List<uint>(b.WalkUints());

            uintsA.Reverse();
            uintsB.Reverse();

            uint carry = 0;

            int headALength = a.head.Length + 1; 
            int headBLength = b.head.Length + 1;
            int maxLength = headALength > headBLength ? headALength : headBLength;

            var newHead = new List<uint>(maxLength);

            for (var i = 0; i < maxLength; i++) {
                ulong sum = carry;
                if (i < headALength)
                    sum += uintsA[i];
                if (i < headBLength)
                    sum += uintsB[i];

                newHead.Add((uint)(sum & 0xFFFFFFFF));
                carry = (uint)(sum >> 32);
            }

            if (carry != 0)
                newHead.Add(carry);

            uint newTail = newHead[0];
            newHead.RemoveAt(0);
            newHead.Reverse();

            return new Natural(newTail, newHead.ToArray());
        }

        public static Integer operator -(Natural a, Natural b) {
            if (b > a)
                return -(b - a);

            var uintsA = new List<uint>(a.WalkUints());
            var uintsB = new List<uint>(b.WalkUints());

            uintsA.Reverse();
            uintsB.Reverse();

            int carry = 0;

            int headALength = a.head.Length + 1;
            int headBLength = b.head.Length + 1;
            int maxLength = headALength > headBLength ? headALength : headBLength;

            var newHead = new List<uint>(maxLength);

            for (var i = 0; i < maxLength; i++) {
                long sum = carry;
                if (i < headALength)
                    sum += uintsA[i];
                if (i < headBLength)
                    sum -= uintsB[i];

                newHead.Add((uint)(Math.Abs(sum) & 0xFFFFFFFF));
                carry = (int)(sum >> 32);
            }

            if (carry != 0)
                newHead.Add((uint) Math.Abs(carry)); 
                // By construction, this value will always be positive.

            uint newTail = newHead[0];
            newHead.RemoveAt(0);
            newHead.Reverse();

            return new Integer((sbyte) 1, new Natural(newTail, newHead.ToArray()));
        }

        public static Natural operator ++(Natural a) {
            return a + 1;
        }
    }
}