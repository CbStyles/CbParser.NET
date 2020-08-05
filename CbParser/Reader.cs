using CbStyles.SrcPos;
using Microsoft.FSharp.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CbStyles.Parser
{
    [CompilationMapping(SourceConstructFlags.Module)]
    public static class Reader
    {
        public static Span<char> reader<a>(a code) where a : IEnumerable<char>
        {
            var r = false;
            IEnumerable<char> f()
            {
                foreach (var c in code)
                {
                    if (c == '\r')
                    {
                        yield return '\n';
                        r = true;
                    } 
                    else if (c == '\n')
                    {
                        if(!r) yield return '\n';
                        r = false;
                    }
                    else
                    {
                        yield return c;
                        r = false;
                    }
                }
            }
            return new Span<char>(f().ToArray());
        }

        public static Span<(Pos, char)> readerPos<a>(a code) where a : IEnumerable<char>
        {
            var r = false;
            var line = 0u;
            var column = 0u;
            IEnumerable<(Pos, char)> f()
            {
                foreach (var c in code)
                {
                    if (c == '\r')
                    {
                        yield return (new Pos(line, column), '\n');
                        line++;
                        column = 0;
                        r = true;
                    }
                    else if (c == '\n')
                    {
                        if (!r)
                        {
                            yield return (new Pos(line, column), '\n');
                            line++;
                            column = 0;
                        }
                        r = false;
                    }
                    else
                    {
                        yield return (new Pos(line, column), c);
                        column++;
                        r = false;
                    }
                }
            }
            return new Span<(Pos, char)>(f().ToArray());
        }

        public static (Span<Pos>, Span<char>) readerPos2<a>(a code) where a : IEnumerable<char>
        {
            var r = false;
            var line = 0u;
            var column = 0u;
            var poss = new List<Pos>();
            var chars = new List<char>();
            foreach (var c in code)
            {
                if (c == '\r')
                {
                    poss.Add(new Pos(line, column));
                    chars.Add('\n');
                    line++;
                    column = 0;
                    r = true;
                }
                else if (c == '\n')
                {
                    if (!r)
                    {
                        poss.Add(new Pos(line, column));
                        chars.Add('\n');
                        line++;
                        column = 0;
                    }
                    r = false;
                }
                else
                {
                    poss.Add(new Pos(line, column));
                    chars.Add(c);
                    column++;
                    r = false;
                }
            }
            return (new Span<Pos>(poss.ToArray()), new Span<char>(chars.ToArray()));
        }
    }
}
