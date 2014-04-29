using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace Bignum {
    public partial struct Natural {
        public static Natural operator *(Natural a, uint b) {
            var uints = new List<uint>(a.WalkUints());
            uints.Reverse();

            uint carry = 0;
            for (var i = 0; i < uints.Count; i++) {
                ulong product = ((ulong)uints[i]) * b + carry;
                uints[i] = (uint)(product & 0xFFFFFFFF);
                carry = (uint)(product >> 32);
            }

            if (carry != 0)
                uints.Add(carry);

            uint newTail = uints[0];
            uints.RemoveAt(0);
            uints.Reverse();

            return new Natural(newTail, uints.ToArray());
        }

        public static Natural operator *(Natural a, Natural b) {
            if (a.head.Length < b.head.Length)
                return b * a;

            List<Natural> partialProducts = new List<Natural>(b.head.Length + 1);

            partialProducts.Add(a * b.tail);

            for (var i = 0; i < b.head.Length; i++) {
                var product = a * b.head[i];
                var newHead = new uint[product.head.Length + i + 1];
                product.head.CopyTo(newHead, 0);
                newHead[product.head.Length] = product.tail;
                product = new Natural(0, newHead);
                partialProducts.Add(product);
            }

            return partialProducts.Aggregate((u, v) => u + v);
        }

        public Natural Factorial() {
            var result = new Natural(1);
            for (Natural i = new Natural(1); i <= this; i++) {
                result = result * i;
            }
            return result;
        }
    }
}