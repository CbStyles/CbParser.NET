using Microsoft.FSharp.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace CbStyles.Parser
{
    [Struct]
    [CustomEquality]
    [NoComparison]
    [CompilationMapping(SourceConstructFlags.ObjectType)]
    [Serializable]
    public struct Span<T> : IEquatable<Span<T>>, IEnumerable<T>
    {
        T[] arr;
        uint from;
        uint to;

        public Span(T[] arr) : this(arr, 0, (uint)arr.Length) { }
        private Span(T[] arr, uint from, uint to)
        {
            this.arr = arr;
            Debug.Assert(to >= from);
            this.from = from;
            this.to = to;
        }

        public T[] GetRawArr() => arr;
        public uint RawFrom => from;
        public uint RawTo => to;
        public int RawIndex(int idx) => (int)from + idx;
        public uint RawIndex(uint idx) => from + idx;

        public uint Length => to - from;
        public int ILength => (int)Length;

        public bool IsEmpty => Length == 0;
        public bool IsNotEmpty => Length > 0;

        public T this[int idx]
        {
            get => arr[RawIndex(idx)];
            set => arr[RawIndex(idx)] = value;
        }

        public T this[uint idx]
        {
            get => arr[RawIndex(idx)];
            set => arr[RawIndex(idx)] = value;
        }

        public FSharpValueOption<T> Get(int idx)
        {
            if (RawIndex(idx) > to)
            {
                return FSharpValueOption<T>.ValueNone;
            }
            return FSharpValueOption<T>.NewValueSome(this[idx]);
        }
        public FSharpValueOption<T> Get(uint idx)
        {
            if (RawIndex(idx) > to)
            {
                return FSharpValueOption<T>.ValueNone;
            }
            return FSharpValueOption<T>.NewValueSome(this[idx]);
        }

        public bool TryGet(int idx, out T val)
        {
            if (RawIndex(idx) > to)
            {
                val = default(T);
                return false;
            }
            val = this[idx];
            return true;
        }
        public bool TryGet(uint idx, out T val)
        {
            if (RawIndex(idx) > to)
            {
                val = default(T);
                return false;
            }
            val = this[idx];
            return true;
        }

        public Span<T> GetSlice(FSharpOption<int> start, FSharpOption<int> end)
        {
            var s = Operators.DefaultArg<int>(start, 0);
            var e = Operators.DefaultArg<int>(end, (int)(to - from));
            return new Span<T>(arr, from + (uint)s, from + (uint)e);
        }

        public Span<T> GetSlice(FSharpOption<uint> start, FSharpOption<uint> end)
        {
            var s = Operators.DefaultArg<uint>(start, 0);
            var e = Operators.DefaultArg<uint>(end, to - from);
            return new Span<T>(arr, from + s, from + e);
        }

        #region Eq

        public override bool Equals(object obj) => obj is Span<T> span && Equals(span);

        public bool Equals(Span<T> other)
        {
            if (other is Span<T> s && Length == s.Length)
            {
                if (Length == 0) return true;
                return !this.Zip(s, (a, b) => (a, b)).Any(v => !v.a.Equals(v.b));
            } 
            else if(other is T[] a && Length == a.Length)
            {
                if (a.Length == 0) return true;
                return !this.Zip(a, (c, b) => (c, b)).Any(v => !v.c.Equals(v.b));
            }
            return false;
        }

        public override int GetHashCode()
        {
            int hashCode = -51588743;
            hashCode = hashCode * -1521134295 + EqualityComparer<T[]>.Default.GetHashCode(this.ToArray());
            return hashCode;
        }

        public static bool operator ==(Span<T> left, Span<T> right) => left.Equals(right);

        public static bool operator !=(Span<T> left, Span<T> right) => !(left == right);

        #endregion

        #region Iter

        public IEnumerator<T> GetEnumerator() => new SpanIter(this);
        IEnumerator IEnumerable.GetEnumerator() => new SpanIter(this);

        private class SpanIter : IEnumerator<T>
        {
            Span<T> span;
            uint i = 0;

            public SpanIter(Span<T> span)
            {
                this.span = span;
            }

            public T Current => span[i - 1];

            object IEnumerator.Current => span[i - 1];

            public void Dispose() { }

            public bool MoveNext()
            {
                if (i >= span.Length) return false;
                i++;
                return true;
            }

            public void Reset() => i = 0;
        }

        #endregion

        public override string ToString()
        {
            return $"Span[{String.Join(", ", this.Select(v => v.ToString()))}]";
        }
    }
}
