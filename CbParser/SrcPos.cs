using Microsoft.FSharp.Core;
using System;
using System.Collections;
using System.Diagnostics;

namespace CbStyles.SrcPos
{
    [Struct]
    [CompilationMapping(SourceConstructFlags.ObjectType)]
    [Serializable]
    public struct Pos : IEquatable<Pos>, IStructuralEquatable, IComparable<Pos>, IComparable, IStructuralComparable
    {
        public nuint line;
        public nuint column;

        public Pos(nuint line, nuint column)
        {
            this.line = line;
            this.column = column;
        }



        #region Eq

        public override bool Equals(object obj) => obj is Pos pos && Equals(pos);
        public bool Equals(Pos other) => line == other.line && column == other.column;
        public bool Equals(object other, IEqualityComparer comparer) => other is Pos pos && Equals(pos);
        public override int GetHashCode()
        {
            int hashCode = 73224550;
            hashCode = hashCode * -1521134295 + line.GetHashCode();
            hashCode = hashCode * -1521134295 + column.GetHashCode();
            return hashCode;
        }
        public int GetHashCode(IEqualityComparer comparer) => GetHashCode();
        public static bool operator ==(Pos left, Pos right) => left.Equals(right);
        public static bool operator !=(Pos left, Pos right) => !(left == right);

        #endregion

        public override string ToString() => $"{{ Pos {line}:{column} }}";

        #region Cmp

        public int CompareTo(Pos other)
        {
            if (line > other.line) return -1;
            if (line < other.line) return 1;
            if (column > other.column) return -1;
            if (column < other.column) return 1;
            return 0;
        }

        public int CompareTo(object obj) => CompareTo((Pos)obj);

        public int CompareTo(object other, IComparer comparer) => CompareTo((Pos)other);

        public static bool operator >(Pos left, Pos right)
        {
            if (left.line > right.line) return true;
            if (left.line < right.line) return false;
            return left.column > right.column;
        }

        public static bool operator <(Pos left, Pos right)
        {
            if (left.line < right.line) return true;
            if (left.line > right.line) return false;
            return left.column < right.column;
        }

        public static bool operator >=(Pos left, Pos right)
        {
            if (left.line > right.line) return true;
            if (left.line < right.line) return false;
            return left.column >= right.column;
        }

        public static bool operator <=(Pos left, Pos right)
        {
            if (left.line < right.line) return true;
            if (left.line > right.line) return false;
            return left.column <= right.column;
        }

        #endregion

    }

    [Struct]
    [CompilationMapping(SourceConstructFlags.ObjectType)]
    [Serializable]
    public struct Loc : IEquatable<Loc>, IStructuralEquatable, IComparable<Loc>, IComparable, IStructuralComparable
    {
        public Pos from;
        public Pos to;

        public Loc(Pos from, Pos to)
        {
            Debug.Assert(from <= to);
            this.from = from;
            this.to = to;
        }
        public Loc(Pos at): this(at, at) { }

        #region Eq

        public override bool Equals(object obj) => obj is Loc loc && Equals(loc);
        public bool Equals(object other, IEqualityComparer comparer) => other is Loc loc && Equals(loc);
        public bool Equals(Loc other) => from.Equals(other.from) && to.Equals(other.to);
        public override int GetHashCode()
        {
            int hashCode = -1951484959;
            hashCode = hashCode * -1521134295 + from.GetHashCode();
            hashCode = hashCode * -1521134295 + to.GetHashCode();
            return hashCode;
        }
        public int GetHashCode(IEqualityComparer comparer) => GetHashCode();
        public static bool operator ==(Loc left, Loc right) => left.Equals(right);
        public static bool operator !=(Loc left, Loc right) => !(left == right);

        #endregion

        public override string ToString() => $"{{ Loc {from.line}:{from.column} .. {to.line}:{to.column} }}";

        #region Cmp

        public int CompareTo(Loc other)
        {
            if (from == to) return 0;
            if (from >= other.from && to >= other.to) return 1;
            if (from <= other.from && to <= other.to) return -1;
            if (from >= other.from && to <= other.to) return -1;
            if (from <= other.from && to >= other.to) return 1;
            return 0;
        }

        public int CompareTo(object obj) => CompareTo((Loc)obj);

        public int CompareTo(object other, IComparer comparer) => CompareTo((Loc)other);

        #endregion

    }
}
