using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace BlowFish.AF.Extensions
{
    public static class UintExtensions
    {
        public static uint RotateRight(this uint value, int rotation)
            => (value >> rotation) | (value << (32 - rotation));
        public static uint ShiftRight(this uint value, int shift)
            => value >> shift;
        public static ulong ShiftLeftToUlong(this uint value, int shift)
            => (ulong)value << shift;
        public static uint Xor(this uint left, uint right)
            => left ^ right;
        public static uint And(this uint left, uint right)
            => left & right;
        public static uint Or(this uint left, uint right)
            => left | right;
        public static uint Not(this uint value)
            => ~value;
        public static uint Add(this uint left, uint right)
            => left + right;

        public static ulong RotateRight(this ulong value, int rotation)
            => (value >> rotation) | (value << (64 - rotation));
        public static ulong ShiftRight(this ulong value, int shift)
            => value >> shift;
        public static ulong ShiftLeft(this ulong value, int shift)
            => value << shift;
        public static ulong Xor(this ulong left, ulong right)
            => left ^ right;
        public static ulong And(this ulong left, ulong right)
            => left & right;
        public static ulong Or(this ulong left, ulong right)
            => left | right;
        public static ulong Not(this ulong value)
            => ~value;

        public static IEnumerable<uint> ShiftRight(this IEnumerable<uint> left, int shift)
            => new uint[shift].Concat(left.Take(left.Count() - shift));
        public static IEnumerable<uint> Add(this IEnumerable<uint> left, IEnumerable<uint> right)
        {
            var rIter = right.GetEnumerator();
            foreach (var l in left)
            {
                if (!rIter.MoveNext()) throw new ArgumentException("Left not equal elements to right");
                yield return l + rIter.Current;
            }

            if (rIter.MoveNext()) throw new ArgumentException("Left not equal elements to right");
        }

        public static IEnumerable<ulong> ShiftRight(this IEnumerable<ulong> left, int shift)
            => new ulong[shift].Concat(left.Take(left.Count() - shift));
        public static IEnumerable<ulong> Add(this IEnumerable<ulong> left, IEnumerable<ulong> right)
        {
            var rIter = right.GetEnumerator();
            foreach (var l in left)
            {
                if (!rIter.MoveNext()) throw new ArgumentException("Left not equal elements to right");
                yield return l + rIter.Current;
            }

            if (rIter.MoveNext()) throw new ArgumentException("Left not equal elements to right");
        }
    }
}
