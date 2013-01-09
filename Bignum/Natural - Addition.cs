using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace Bignum {
    public partial struct Natural {
        public static Natural operator +(Natural a, Natural b) {
            ulong sum = ((ulong)a.tail) + b.tail;
            uint newTail = (uint) (sum & 0xFFFFFFFF);
            uint carry = (uint) (sum >> 32);

            int headALength = a.head.Length;
            int headBLength = b.head.Length;
            int maxLength = headALength > headBLength ? headALength : headBLength;

            var newHead = new List<uint>(maxLength);

            for (var i = 0; i < maxLength; i++) {
                sum = carry;
                if (i < headALength)
                    sum += a.head[i];
                if (i < headBLength)
                    sum += b.head[i];

                newHead.Add((uint)(sum & 0xFFFFFFFF));
                carry = (uint)(sum >> 32);
            }

            if (carry != 0)
                newHead.Add(carry);

            return new Natural(newTail, newHead.ToArray());
        }

        public static Integer operator -(Natural a, Natural b) {
            if (b > a)
                return -(b - a);

            long sum = ((long)a.tail) - b.tail;
            uint newTail = (uint)(Math.Abs(sum) & 0xFFFFFFFF);
            int carry = (int)(sum >> 32);

            int headALength = a.head.Length;
            int headBLength = b.head.Length;
            int maxLength = headALength > headBLength ? headALength : headBLength;

            var newHead = new List<uint>(maxLength);

            for (var i = 0; i < maxLength; i++) {
                sum = carry;
                if (i < headALength)
                    sum += a.head[i];
                if (i < headBLength)
                    sum -= b.head[i];

                newHead.Add((uint)(Math.Abs(sum) & 0xFFFFFFFF));
                carry = (int)(sum >> 32);
            }

            if (carry != 0)
                newHead.Add((uint) Math.Abs(carry)); 
                // By construction, this value will always be positive.

            return new Integer((sbyte) 1, new Natural(newTail, newHead.ToArray()));
        }
    }
}