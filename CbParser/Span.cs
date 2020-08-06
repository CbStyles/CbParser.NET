#nullable enable
using Microsoft.FSharp.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
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
        internal T[] arr;
        internal uint from;
        internal uint to;

        public Span(T[] arr) : this(arr, 0, (uint)arr.Length) { }
        internal Span(T[] arr, uint from, uint to)
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

        public uint ULength => to - from;
        public int Length => (int)ULength;

        public bool IsEmpty => ULength == 0;
        public bool IsNotEmpty => ULength > 0;

        [NotNull]
        public T this[int idx]
        {
            get => arr[RawIndex(idx)]!;
            set => arr[RawIndex(idx)] = value;
        }

        [NotNull]
        public T this[uint idx]
        {
            get => arr[RawIndex(idx)]!;
            set => arr[RawIndex(idx)] = value;
        }

        public bool CanGet(int idx) => RawIndex(idx) < to;

        public bool CanGet(uint idx) => RawIndex(idx) < to;

        public FSharpValueOption<T> TryGet(int idx)
        {
            if (!CanGet(idx)) return FSharpValueOption<T>.ValueNone;
            return FSharpValueOption<T>.NewValueSome(this[idx]);
        }

        public FSharpValueOption<T> TryGet(uint idx)
        {
            if (!CanGet(idx)) return FSharpValueOption<T>.ValueNone;
            return FSharpValueOption<T>.NewValueSome(this[idx]);
        }

        public bool TryGet(int idx, [MaybeNull, NotNullWhen(true)] out T val)
        {
            if (!CanGet(idx))
            {
                val = default;
                return false;
            }
            val = this[idx];
            return true;
        }
        public bool TryGet(uint idx, [MaybeNull, NotNullWhen(true)] out T val)
        {
            if (!CanGet(idx))
            {
                val = default;
                return false;
            }
            val = this[idx];
            return true;
        }

        public Span<T> GetSlice(FSharpOption<int> start, FSharpOption<int> end)
        {
            var s = Operators.DefaultArg<int>(start, 0);
            var e = Operators.DefaultArg<int>(end, Length);
            Debug.Assert(s <= e);
            return new Span<T>(arr, from + (uint)s, from + (uint)e);
        }

        public Span<T> GetSlice(FSharpOption<uint> start, FSharpOption<uint> end)
        {
            var s = Operators.DefaultArg<uint>(start, 0);
            var e = Operators.DefaultArg<uint>(end, ULength);
            Debug.Assert(s <= e);
            return new Span<T>(arr, from + s, from + e);
        }

        public Span<T> Slice(int idx, int len)
        {
            Debug.Assert(idx >= 0);
            Debug.Assert(len >= idx);
            var s = from + (uint)idx;
            return new Span<T>(arr, s, s + (uint)len);
        }

        public Span<T> Slice(uint idx, uint len)
        {
            Debug.Assert(len >= idx);
            var s = from + idx;
            return new Span<T>(arr, s, s + len);
        }

        #region Eq

        public override bool Equals(object obj) => obj is Span<T> span && Equals(span);

        public bool Equals(Span<T> other)
        {
            if (other is Span<T> s && Length == s.Length)
            {
                if (Length == 0) return true;
                return !this.Zip(s, (a, b) => (a, b)).Any(v => !v.a?.Equals(v.b) ?? !v.b?.Equals(v.a) ?? true);
            } 
            else if(other is T[] a && Length == a.Length)
            {
                if (a.Length == 0) return true;
                return !this.Zip(a, (c, b) => (c, b)).Any(v => !v.c?.Equals(v.b) ?? !v.b?.Equals(v.c) ?? true);
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
                if (i >= span.ULength) return false;
                i++;
                return true;
            }

            public void Reset() => i = 0;
        }

        #endregion

        public override string ToString()
        {
            return $"Span[{String.Join(", ", this.Select(v => v?.ToString()))}]";
        }
    }

    public static class SpanExtNotNull
    {
        public static T? Get<T>(this Span<T> self, int idx) where T : struct
        {
            if (!self.CanGet(idx)) return null;
            return self[idx];
        }

        public static T? Get<T>(this Span<T> self, uint idx) where T : struct
        {
            if (!self.CanGet(idx)) return null;
            return self[idx];
        }
    }
    public static class SpanExtNull
    {
        public static T? Get<T>(this Span<T> self, int idx) where T : class
        {
            if (!self.CanGet(idx)) return null;
            return self[idx];
        }

        public static T? Get<T>(this Span<T> self, uint idx) where T : class
        {
            if (!self.CanGet(idx)) return null;
            return self[idx];
        }
    }
}
