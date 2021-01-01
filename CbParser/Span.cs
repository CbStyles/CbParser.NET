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
        internal nuint from;
        internal nuint to;

        public Span(T[] arr) : this(arr, 0, (nuint)arr.Length) { }
        internal Span(T[] arr, nuint from, nuint to)
        {
            this.arr = arr;
            Debug.Assert(to >= from);
            this.from = from;
            this.to = to;
        }

        public T[] GetRawArr() => arr;
        public nuint RawFrom => from;
        public nuint RawTo => to;
        public int RawIndex(int idx) => (int)from + idx;
        public uint RawIndex(uint idx) => (uint)(from + idx);
        public nint RawIndex(nint idx) => (nint)from + idx;
        public nuint RawIndex(nuint idx) => from + idx;

        public int Length => (int)NULength;
        public uint ULength => (uint)NULength;
        public nint NLength => (nint)NULength;
        public nuint NULength => to - from;
        
        public bool IsEmpty => NULength == 0;
        public bool IsNotEmpty => NULength > 0;

        public T this[int idx]
        {
            get => arr[RawIndex(idx)]!;
            set => arr[RawIndex(idx)] = value;
        }

        public T this[uint idx]
        {
            get => arr[RawIndex(idx)]!;
            set => arr[RawIndex(idx)] = value;
        }
        public T this[nint idx]
        {
            get => arr[RawIndex(idx)]!;
            set => arr[RawIndex(idx)] = value;
        }

        public T this[nuint idx]
        {
            get => arr[RawIndex(idx)]!;
            set => arr[RawIndex(idx)] = value;
        }

        public bool CanGet(int idx) => CanGet((nuint)idx);

        public bool CanGet(uint idx) => CanGet((nuint)idx);

        public bool CanGet(nint idx) => CanGet((nuint)idx);

        public bool CanGet(nuint idx) => RawIndex(idx) < to;


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

        public FSharpValueOption<T> TryGet(nint idx)
        {
            if (!CanGet(idx)) return FSharpValueOption<T>.ValueNone;
            return FSharpValueOption<T>.NewValueSome(this[idx]);
        }

        public FSharpValueOption<T> TryGet(nuint idx)
        {
            if (!CanGet(idx)) return FSharpValueOption<T>.ValueNone;
            return FSharpValueOption<T>.NewValueSome(this[idx]);
        }

        public bool TryGet(int idx, out T val)
        {
            if (!CanGet(idx))
            {
#pragma warning disable CS8601 // 可能的 null 引用赋值。
                val = default;
#pragma warning restore CS8601 // 可能的 null 引用赋值。
                return false;
            }
            val = this[idx];
            return true;
        }
        public bool TryGet(uint idx, out T val)
        {
            if (!CanGet(idx))
            {
#pragma warning disable CS8601 // 可能的 null 引用赋值。
                val = default;
#pragma warning restore CS8601 // 可能的 null 引用赋值。
                return false;
            }
            val = this[idx];
            return true;
        }
        public bool TryGet(nint idx, out T val)
        {
            if (!CanGet(idx))
            {
#pragma warning disable CS8601 // 可能的 null 引用赋值。
                val = default;
#pragma warning restore CS8601 // 可能的 null 引用赋值。
                return false;
            }
            val = this[idx];
            return true;
        }
        public bool TryGet(nuint idx, out T val)
        {
            if (!CanGet(idx))
            {
#pragma warning disable CS8601 // 可能的 null 引用赋值。
                val = default;
#pragma warning restore CS8601 // 可能的 null 引用赋值。
                return false;
            }
            val = this[idx];
            return true;
        }

        public Span<T> GetSlice(FSharpOption<int> start, FSharpOption<int> end)
        {
            var s = Operators.DefaultArg(start, 0);
            var e = Operators.DefaultArg(end, Length);
            Debug.Assert(s <= e);
            return new Span<T>(arr, from + (uint)s, from + (uint)e);
        }

        public Span<T> GetSlice(FSharpOption<uint> start, FSharpOption<uint> end)
        {
            var s = Operators.DefaultArg<uint>(start, 0);
            var e = Operators.DefaultArg(end, ULength);
            Debug.Assert(s <= e);
            return new Span<T>(arr, from + s, from + e);
        }

        public Span<T> GetSlice(FSharpOption<nint> start, FSharpOption<nint> end)
        {
            var s = Operators.DefaultArg(start, 0);
            var e = Operators.DefaultArg(end, Length);
            Debug.Assert(s <= e);
            return new Span<T>(arr, from + (uint)s, from + (uint)e);
        }

        public Span<T> GetSlice(FSharpOption<nuint> start, FSharpOption<nuint> end)
        {
            var s = Operators.DefaultArg<nuint>(start, 0);
            var e = Operators.DefaultArg(end, ULength);
            Debug.Assert(s <= e);
            return new Span<T>(arr, from + s, from + e);
        }

        public Span<T> Slice(int idx, int len)
        {
            Debug.Assert(idx >= 0);
            Debug.Assert(len >= idx);
            var s = from + (nuint)idx;
            return new Span<T>(arr, s, s + (nuint)len);
        }
        public Span<T> Slice(uint idx, uint len)
        {
            Debug.Assert(len >= idx);
            var s = from + idx;
            return new Span<T>(arr, s, s + len);
        }
        public Span<T> Slice(nint idx, nint len)
        {
            Debug.Assert(idx >= 0);
            Debug.Assert(len >= idx);
            var s = from + (nuint)idx;
            return new Span<T>(arr, s, s + (nuint)len);
        }
        public Span<T> Slice(nuint idx, nuint len)
        {
            Debug.Assert(len >= idx);
            var s = from + idx;
            return new Span<T>(arr, s, s + len);
        }

        #region Eq

#pragma warning disable CS8765 // 参数类型的为 Null 性与重写成员不匹配(可能是由于为 Null 性特性)。
        public override bool Equals(object obj) => obj is Span<T> span && Equals(span);
#pragma warning restore CS8765 // 参数类型的为 Null 性与重写成员不匹配(可能是由于为 Null 性特性)。

        public bool Equals(Span<T> other)
        {
            if (other is Span<T> s && NULength == s.NULength)
            {
                if (NULength == 0) return true;
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
            nuint i = 0;

            public SpanIter(Span<T> span)
            {
                this.span = span;
            }

            public T Current => span[i - 1];

#pragma warning disable CS8603 // 可能的 null 引用返回。
            object IEnumerator.Current => span[i - 1];
#pragma warning restore CS8603 // 可能的 null 引用返回。

            public void Dispose() { }

            public bool MoveNext()
            {
                if (i >= span.NULength) return false;
                i++;
                return true;
            }

            public void Reset() => i = 0;
        }

        #endregion

        public override string ToString()
        {
            return $"Span[{string.Join(", ", this.Select(v => v?.ToString()))}]";
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

        public static T? Get<T>(this Span<T> self, nint idx) where T : struct
        {
            if (!self.CanGet(idx)) return null;
            return self[idx];
        }

        public static T? Get<T>(this Span<T> self, nuint idx) where T : struct
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

        public static T? Get<T>(this Span<T> self, nint idx) where T : class
        {
            if (!self.CanGet(idx)) return null;
            return self[idx];
        }

        public static T? Get<T>(this Span<T> self, nuint idx) where T : class
        {
            if (!self.CanGet(idx)) return null;
            return self[idx];
        }
    }
}
