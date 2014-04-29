using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace Bignum {
    public partial struct Natural {
        IEnumerable<uint> WalkUints() {
            foreach (var item in head)
                yield return item;

            yield return tail;
        }

        IEnumerable<bool> WalkBits() {
            var oneSeen = false;

            foreach (var item in WalkUints()) {
                var mask = 0x80000000;
                while (mask > 0) {
                    var bit = (item & mask) != 0;
                    if (bit)
                        oneSeen = true;

                    mask = mask >> 1;
                    if (!oneSeen)
                        continue;
                    yield return bit;
                }
            }
        }

        public static void Divide(Natural a, Natural b, out Natural quotient, out Natural remainder) {
            // Taken from http://en.wikipedia.org/wiki/Division_algorithm#Integer_division_.28unsigned.29_with_remainder

            if (b == 0)
                throw new DivideByZeroException();

            remainder = 0;

            List<bool> q = new List<bool>();
            foreach (var bit in a.WalkBits()) {
                remainder *= 2;

                if (bit)
                    remainder += 1;

                if (remainder >= b) {
                    remainder = (remainder - b).Magnitude;
                    q.Add(true);
                }
                else
                    q.Add(false);
            }

            List<uint> uints = new List<uint>();
            uint current = 0;
            uint currentMask = 0x1;
            while (q.Count > 0) {
                if (currentMask == 0) {
                    uints.Add(current);
                    currentMask = 0x1;
                    current = 0;
                }
               
                if (q[q.Count - 1])
                    current |= currentMask;
                
                q.RemoveAt(q.Count - 1);
                currentMask = currentMask << 1;
            }

            uints.Add(current);

            if (uints.Count == 0)
                quotient = 0;
            else {
                var tail = uints[0];
                uints.RemoveAt(0);
                uints.Reverse();
                while (uints.Count > 0 && uints[0] == 0)
                    uints.RemoveAt(0);

                quotient = new Natural(tail, uints.ToArray());
            }
        }

        public static Natural operator /(Natural a, Natural b) {
            Natural quotient, remainder;
            Divide(a, b, out quotient, out remainder);
            return quotient;
        }

        public static Natural operator %(Natural a, Natural b) {
            Natural quotient, remainder;
            Divide(a, b, out quotient, out remainder);
            return remainder;
        }
    }
}