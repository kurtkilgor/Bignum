using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace Bignum {
    public partial struct Natural {
        public static Natural operator *(Natural a, uint b) {
            ulong product = ((ulong)a.tail) * b;
            uint newTail = (uint)(product & 0xFFFFFFFF);
            uint carry = (uint)(product >> 32);

            var newHead = new List<uint>(a.head.Length);

            for (var i = 0; i < a.head.Length; i++) {
                product = ((ulong)a.head[i]) * b + carry;
                newHead.Add((uint)(product & 0xFFFFFFFF));
                carry = (uint)(product >> 32);
            }

            if (carry != 0)
                newHead.Add(carry);

            return new Natural(newTail, newHead.ToArray());
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
    }
}